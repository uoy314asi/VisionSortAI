using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using Microsoft.Win32;
using NeuroVisionDP.Core;

namespace NeuroVisionDP.MVVM.ViewModel
{
    public class DiscoveryViewModel : ObservableObject
    {
        private string _resultText;
        public string ResultText
        {
            get { return _resultText; }
            set
            {
                _resultText = value;
                OnPropertyChanged();
            }
        }

        private bool _isProcessing;
        public bool IsProcessing
        {
            get { return _isProcessing; }
            set
            {
                _isProcessing = value;
                OnPropertyChanged();
            }
        }

        private BitmapImage _uploadedImage;
        public BitmapImage UploadedImage
        {
            get { return _uploadedImage; }
            set
            {
                _uploadedImage = value;
                OnPropertyChanged();
            }
        }

        private readonly HomeViewModel _homeViewModel;

        public ICommand UploadImageCommand { get; }

        public string PythonPath { get; set; }
        public string ScriptPath { get; set; }

        // Конструктор по умолчанию
        public DiscoveryViewModel()
        {
            InitializePaths();
        }

        // Конструктор с параметром HomeViewModel
        public DiscoveryViewModel(HomeViewModel homeViewModel)
        {
            _homeViewModel = homeViewModel ?? throw new ArgumentNullException(nameof(homeViewModel));
            UploadImageCommand = new RelayCommand(async () => await UploadImageAsync());
            InitializePaths();
        }

        private void InitializePaths()
        {
            var baseDirectory = Directory.GetCurrentDirectory();
            PythonPath = Path.Combine(baseDirectory, "venv", "Scripts", "python.exe");
            ScriptPath = Path.Combine(baseDirectory, "upload_image_and_get_tags.py");
        }

        public async Task UploadImageAsync()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "Image files (*.png;*.jpg)|*.png;*.jpg|All files (*.*)|*.*"
            };

            if (openFileDialog.ShowDialog() == true)
            {
                var filePath = openFileDialog.FileName;
                IsProcessing = true;
                var result = await Task.Run(() => UploadImageAndGetTags(filePath));
                ResultText = result;
                IsProcessing = false;

                _homeViewModel?.LoadImages();

                var bitmap = new BitmapImage();
                bitmap.BeginInit();
                bitmap.UriSource = new Uri(filePath, UriKind.Absolute);
                bitmap.CacheOption = BitmapCacheOption.OnLoad;
                bitmap.EndInit();
                UploadedImage = bitmap;
            }
        }

        public virtual string UploadImageAndGetTags(string filePath)
        {
            if (!File.Exists(PythonPath))
            {
                return $"Error: Python interpreter not found at {PythonPath}";
            }
            if (!File.Exists(ScriptPath))
            {
                return $"Error: Python script not found at {ScriptPath}";
            }

            try
            {
                var start = new ProcessStartInfo
                {
                    FileName = PythonPath,
                    Arguments = $"\"{ScriptPath}\" \"{filePath}\"",
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    CreateNoWindow = true,
                    WorkingDirectory = Directory.GetCurrentDirectory(),
                    StandardOutputEncoding = Encoding.UTF8,
                    StandardErrorEncoding = Encoding.UTF8
                };

                using (var process = Process.Start(start))
                {
                    using (var reader = process.StandardOutput)
                    {
                        string result = reader.ReadToEnd();
                        string errors = process.StandardError.ReadToEnd();

                        if (!string.IsNullOrEmpty(errors))
                        {
                            return $"Error: {errors}";
                        }

                        if (string.IsNullOrEmpty(result))
                        {
                            return "No output from Python script.";
                        }

                        return "Image successfully uploaded and sorted.";
                    }
                }
            }
            catch (Exception ex)
            {
                return $"Error: {ex.Message}";
            }
        }
    }
}

using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows.Media.Imaging;
using NeuroVisionDP.Core;

namespace NeuroVisionDP.MVVM.ViewModel
{
    public class ImageInfo
    {
        public BitmapImage Image { get; set; }
        public string FileName { get; set; }
    }

    public class HomeViewModel : ObservableObject
    {
        private ObservableCollection<ImageInfo> _images;
        public ObservableCollection<ImageInfo> Images
        {
            get { return _images; }
            set
            {
                _images = value;
                OnPropertyChanged();
            }
        }

        private string _searchText;
        public string SearchText
        {
            get { return _searchText; }
            set
            {
                _searchText = value;
                OnPropertyChanged();
                FilterImages();
            }
        }

        private ObservableCollection<ImageInfo> _allImages;

        public HomeViewModel()
        {
            try
            {
                LoadImages();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading images: {ex.Message}");
            }
        }

        // Конструктор для тестирования
        public HomeViewModel(ObservableCollection<ImageInfo> testImages)
        {
            _allImages = testImages;
            Images = new ObservableCollection<ImageInfo>(_allImages);
        }

        public void LoadImages()
        {
            string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
            string imagesDirectory = Path.Combine(baseDirectory, "SortedImages");

            if (Directory.Exists(imagesDirectory))
            {
                var imageFiles = Directory.GetFiles(imagesDirectory, "*.*", SearchOption.AllDirectories)
                                          .Where(file => file.EndsWith(".jpg", StringComparison.OrdinalIgnoreCase) ||
                                                         file.EndsWith(".png", StringComparison.OrdinalIgnoreCase) ||
                                                         file.EndsWith(".jpeg", StringComparison.OrdinalIgnoreCase))
                                          .ToList();

                _allImages = new ObservableCollection<ImageInfo>();

                foreach (var imageFile in imageFiles)
                {
                    try
                    {
                        var bitmap = new BitmapImage();
                        bitmap.BeginInit();
                        bitmap.UriSource = new Uri(imageFile, UriKind.Absolute);
                        bitmap.CacheOption = BitmapCacheOption.OnLoad;
                        bitmap.EndInit();

                        var imageInfo = new ImageInfo
                        {
                            Image = bitmap,
                            FileName = Path.GetFileName(imageFile)
                        };

                        _allImages.Add(imageInfo);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error loading image {imageFile}: {ex.Message}");
                    }
                }

                Images = new ObservableCollection<ImageInfo>(_allImages);
            }
            else
            {
                Console.WriteLine($"Directory not found: {imagesDirectory}");
            }
        }

        public void FilterImages()
        {
            if (string.IsNullOrEmpty(SearchText))
            {
                Images = new ObservableCollection<ImageInfo>(_allImages);
            }
            else
            {
                var filteredImages = _allImages
                    .Where(img => img.FileName.IndexOf(SearchText, StringComparison.OrdinalIgnoreCase) >= 0)
                    .ToList();

                Images = new ObservableCollection<ImageInfo>(filteredImages);
            }
        }
    }
}

using System.Windows.Input;
using NeuroVisionDP.Core;

namespace NeuroVisionDP.MVVM.ViewModel
{
    public class SettingsViewModel : ObservableObject
    {
        private string _imagePath;
        public string ImagePath
        {
            get { return _imagePath; }
            set
            {
                _imagePath = value;
                OnPropertyChanged();
            }
        }

        private string _selectedLanguage;
        public string SelectedLanguage
        {
            get { return _selectedLanguage; }
            set
            {
                _selectedLanguage = value;
                OnPropertyChanged();
            }
        }

        private string _selectedTheme;
        public string SelectedTheme
        {
            get { return _selectedTheme; }
            set
            {
                _selectedTheme = value;
                OnPropertyChanged();
            }
        }

        private bool _enableNotifications;
        public bool EnableNotifications
        {
            get { return _enableNotifications; }
            set
            {
                _enableNotifications = value;
                OnPropertyChanged();
            }
        }

        private int _notificationVolume;
        public int NotificationVolume
        {
            get { return _notificationVolume; }
            set
            {
                _notificationVolume = value;
                OnPropertyChanged();
            }
        }

        private int _autoSaveInterval;
        public int AutoSaveInterval
        {
            get { return _autoSaveInterval; }
            set
            {
                _autoSaveInterval = value;
                OnPropertyChanged();
            }
        }

        public ICommand SaveSettingsCommand { get; set; }

        public SettingsViewModel()
        {
            // Инициализация команды сохранения настроек
            SaveSettingsCommand = new RelayCommand(SaveSettings);
        }

        private void SaveSettings()
        {
            // Логика сохранения настроек
        }
    }
}

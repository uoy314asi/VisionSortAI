using NeuroVisionDP.Core;
using System;
using System.Windows;

namespace NeuroVisionDP.MVVM.ViewModel
{
    class MainViewModel : ObservableObject
    {
        public RelayCommand HomeViewCommand { get; set; }
        public RelayCommand DiscoveryViewCommand { get; set; }
        public RelayCommand SettingsViewCommand { get; set; }
        public RelayCommand MinimizeCommand { get; set; }
        public RelayCommand MaximizeCommand { get; set; }
        public RelayCommand CloseCommand { get; set; }

        public HomeViewModel HomeVM { get; set; }
        public DiscoveryViewModel DiscoveryVM { get; set; }
        public SettingsViewModel SettingsVM { get; set; }

        private object _currentView;

        public object CurrentView
        {
            get { return _currentView; }
            set
            {
                _currentView = value;
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
                Console.WriteLine($"MainViewModel: SearchText updated to {_searchText}");
                HomeVM.SearchText = value;
            }
        }

        public MainViewModel()
        {
            HomeVM = new HomeViewModel();
            DiscoveryVM = new DiscoveryViewModel(HomeVM);
            SettingsVM = new SettingsViewModel();
            CurrentView = HomeVM;

            HomeViewCommand = new RelayCommand(o =>
            {
                CurrentView = HomeVM;
            });

            DiscoveryViewCommand = new RelayCommand(o =>
            {
                CurrentView = DiscoveryVM;
            });

            SettingsViewCommand = new RelayCommand(o =>
            {
                CurrentView = SettingsVM;
            });

            MinimizeCommand = new RelayCommand(o =>
            {
                App.Current.MainWindow.WindowState = WindowState.Minimized;
            });

            MaximizeCommand = new RelayCommand(o =>
            {
                if (App.Current.MainWindow.WindowState == WindowState.Maximized)
                {
                    App.Current.MainWindow.WindowState = WindowState.Normal;
                }
                else
                {
                    App.Current.MainWindow.WindowState = WindowState.Maximized;
                }
            });

            CloseCommand = new RelayCommand(o =>
            {
                App.Current.MainWindow.Close();
            });
        }
    }
}

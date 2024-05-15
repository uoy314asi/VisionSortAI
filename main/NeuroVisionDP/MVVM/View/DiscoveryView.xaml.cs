using System.Windows.Controls;
using NeuroVisionDP.MVVM.ViewModel;

namespace NeuroVisionDP.MVVM.View
{
    public partial class DiscoveryView : UserControl
    {
        public DiscoveryView()
        {
            InitializeComponent();
            var mainViewModel = App.Current.MainWindow.DataContext as MainViewModel;
            DataContext = new DiscoveryViewModel(mainViewModel.HomeVM);
        }
    }
}

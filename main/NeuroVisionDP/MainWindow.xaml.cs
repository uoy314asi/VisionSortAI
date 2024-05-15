using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace NeuroVisionDP
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            this.PreviewMouseDown += MainWindow_PreviewMouseDown; // Привязка обработчика события
        }

        private void MainWindow_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left && e.ButtonState == MouseButtonState.Pressed && e.Source == MyBorder)
            {
                try
                {
                    this.DragMove();
                }
                catch (InvalidOperationException ex)
                {
                    // Игнорируем ошибку, если она возникает, когда нельзя перемещать окно
                }
            }
        }



        private void RadioButton_Checked(object sender, RoutedEventArgs e)
        {

        }
    }
}

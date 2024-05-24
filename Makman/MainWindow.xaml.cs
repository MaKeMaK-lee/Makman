
using Makman.Visual.Localization;
using Makman.Visual.MVVM.Model;
using System.Windows;
using System.Windows.Input;

namespace Makman
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public required IServicesAccessor _servicesAccessor;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            e.Cancel = true;
            if (_servicesAccessor.SaveDatabase() && _servicesAccessor.SaveSettings())
                e.Cancel = false;
            else
            {
                var result = MessageBox.Show(UIText.mw_warning_save_message, UIText.mw_warning_save_caption, MessageBoxButton.YesNo, MessageBoxImage.Warning, MessageBoxResult.No);
                if (result == MessageBoxResult.Yes)
                {
                    e.Cancel = false;
                    return;
                }
            }
        }

        private void Window_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (Mouse.LeftButton == MouseButtonState.Pressed && Mouse.GetPosition(this).Y <= 32)
            {
                this.DragMove();
            }
        }
    }
}


using Makman.Middle.Services;//TODO 500 change
using Makman.Visual.Localization; 
using System.Windows;
using System.Windows.Input;

namespace Makman
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public required ICollectionDatabaseService collectionDatabaseService;
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
            if (collectionDatabaseService.Save())
                e.Cancel = false;
            else
            {
                var result = MessageBox.Show(UIText.u_no_text, UIText.u_no_text, MessageBoxButton.YesNo, MessageBoxImage.Warning, MessageBoxResult.No);
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

/*

 
 using Makman.Entities;

namespace Makman
{
    public static class ListExtension
    {
        public static void AddFilesFromFolder(this List<Unit> collection, string directoryPath, bool allSubDirectories = false)
        {
            IEnumerable<string> filesToAdd;
            if (allSubDirectories)
                filesToAdd = WindowsUse.GetFilesFromDirectory(directoryPath);
            else
                filesToAdd = WindowsUse.GetFilesFromDirectory(directoryPath);

            List<Unit> tmpCollection = [];
            foreach (var file in filesToAdd)
            {
                tmpCollection.Add(new(file));
            }
            collection.AddRange(tmpCollection);
        }
    }
}

 
 
 
 
 
 * 
 */
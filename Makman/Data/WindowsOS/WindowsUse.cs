using Microsoft.Win32;
using Microsoft.WindowsAPICodePack.Shell;
using System.Diagnostics;
using System.IO;
using System.Windows.Media.Imaging;

namespace Makman.Data.WindowsOS
{
    static internal class WindowsUse
    {
        static internal string? ChooseDirectory()
        {
            OpenFolderDialog dialog = new();
            dialog.Multiselect = false;
            dialog.Title = "Select a directory";

            bool? result = dialog.ShowDialog();

            if (result == true)
                return dialog.FolderName;
            else
                return null;
        }

        static internal IEnumerable<string> GetDirectoriesFromDirectory(string directoryPath)
        {
            return Directory.EnumerateDirectories(directoryPath);
        }

        static internal IEnumerable<string> GetFilesFromDirectory(string directoryPath)
        {
            return Directory.EnumerateFiles(directoryPath);
        }

        static internal IEnumerable<string> GetFilesFromDirectoryAndChilderns(string directoryPath)
        {
            return Directory.EnumerateFiles(directoryPath, "*", System.IO.SearchOption.AllDirectories);
        }

        static internal void ViewInExplorer(string path)
        {
            path.Replace("\\", "/");
            Process.Start(new ProcessStartInfo { FileName = "explorer", Arguments = $"/n, /select, {path}" });
        }

        static internal bool FileExists(string fileName)
        {
            return File.Exists(fileName);
        }

        static internal string ReadAllText(string fileName)
        {
            return File.ReadAllText(fileName);
        }

        static internal void WriteAllText(string fileName, string text)
        {
            File.WriteAllText(fileName, text);
        }

        static internal DateTime GetFileLastWriteTime(string path)
        {
            return new FileInfo(path).LastWriteTime;
        }

        static internal long GetFileSize(string path)
        {
            return new FileInfo(path).Length;
        }

        static internal BitmapImage? TryGetImageOfFile(string path)
        {
            try
            {
                return new BitmapImage(new Uri(path));
            }
            catch (Exception)
            {
                return null;
            }
        }

        static internal object GetImageOfFile(string path, bool justThumbnail)
        {
            var shellObject = ShellObject.FromParsingName(path);

            if (!justThumbnail)
            {
                string contentType = (string)shellObject.Properties.GetProperty("System.ContentType").ValueAsObject;
                if (contentType.StartsWith("image"))
                {
                    var imageResult = TryGetImageOfFile(path);
                    if (imageResult != null)
                        return imageResult;
                }
                return shellObject.Thumbnail.ExtraLargeBitmapSource;
            }
            return shellObject.Thumbnail.MediumBitmapSource;

        }
    }
}

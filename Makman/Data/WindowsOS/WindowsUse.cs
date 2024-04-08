using Microsoft.Win32;
using System.IO;

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

        static internal bool FileExists(string fileName)
        {
            return File.Exists(fileName);
        }

        static internal string ReadAllText(string fileName)
        {
            return File.ReadAllText(fileName);
        }

        static internal void WriteAllText(string fileName, string jsonString)
        {
            File.WriteAllText(fileName, jsonString);
        }
    }
}

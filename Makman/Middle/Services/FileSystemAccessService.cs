
using Makman.Data.WindowsOS; 

namespace Makman.Middle.Services
{
    public class FileSystemAccessService : IFileSystemAccessService
    {
        public string? ChooseDirectory()
        {
            return WindowsUse.ChooseDirectory();
        }

        public bool FileExists(string fileName)
        {
            return WindowsUse.FileExists(fileName);
        }

        public DateTime GetFileLastWriteTime(string path)
        {
            return WindowsUse.GetFileLastWriteTime(path);
        }

        public long GetFileSize(string path)
        {
            return WindowsUse.GetFileSize(path);
        }

        public IEnumerable<string> GetFilesFromDirectory(string directoryPath)
        {
            return WindowsUse.GetFilesFromDirectory(directoryPath);
        }

        public IEnumerable<string> GetFilesFromDirectoryAndChilderns(string directoryPath)
        {
            return WindowsUse.GetFilesFromDirectoryAndChilderns(directoryPath);
        }

        public object GetImageOfFile(string path, bool justThumbnail)
        {
            return WindowsUse.GetImageOfFile(path, justThumbnail);
        }

        public void ViewInExplorer(string path)
        {
            WindowsUse.ViewInExplorer(path);
        }

    }
}

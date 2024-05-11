
namespace Makman.Middle.Services
{
    public interface IFileSystemAccessService
    {
        /// <summary>
        /// Opens the directory selection window
        /// </summary> 
        string? ChooseDirectory();

        /// <returns>Full filenames of all files in directory</returns>
        IEnumerable<string> GetFilesFromDirectory(string directoryPath);

        /// <returns>Full filenames of all files in directory and all subdirectories</returns>
        IEnumerable<string> GetFilesFromDirectoryAndChilderns(string directoryPath);

        long GetFileSize(string path);

        DateTime GetFileLastWriteTime(string path);

        void ViewInExplorer(string path);

        bool FileExists(string fileName);

        string ReadAllText(string fileName);

        void WriteAllText(string fileName, string text);

        object GetImageOfFile(string path, bool justThumbnail);
    }
}

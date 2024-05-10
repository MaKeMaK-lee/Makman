 
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

        void ViewInExplorer(string path);

        bool FileExists(string fileName);

        object GetImageOfFile(string path, bool justThumbnail);
    }
}

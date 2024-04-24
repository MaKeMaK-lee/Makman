
using Makman.Middle.Entities;
using System.Windows.Media.Imaging;

namespace Makman.Middle.Services
{
    public interface IFileSystemAccessService
    {
        /// <summary>
        /// Opens the directory selection window
        /// </summary> 
        public string? ChooseDirectory();

        /// <returns>Full filenames of all files in directory</returns>
        public IEnumerable<string> GetFilesFromDirectory(string directoryPath);

        /// <returns>Full filenames of all files in directory and all subdirectories</returns>
        public IEnumerable<string> GetFilesFromDirectoryAndChilderns(string directoryPath); 
    }
}

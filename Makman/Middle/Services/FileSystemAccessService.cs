﻿
using Makman.Data.WindowsOS; 

namespace Makman.Middle.Services
{
    public class FileSystemAccessService : IFileSystemAccessService
    {
        public string? ChooseDirectory()
        {
            return WindowsUse.ChooseDirectory();
        }

        public IEnumerable<string> GetFilesFromDirectory(string directoryPath)
        {
            return WindowsUse.GetFilesFromDirectory(directoryPath);
        }

        public IEnumerable<string> GetFilesFromDirectoryAndChilderns(string directoryPath)
        {
            return WindowsUse.GetFilesFromDirectoryAndChilderns(directoryPath);
        }
    }
}
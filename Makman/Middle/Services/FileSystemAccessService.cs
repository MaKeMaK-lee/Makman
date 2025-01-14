﻿
using Makman.Data.WindowsOS;
using Makman.Middle.Utilities;

namespace Makman.Middle.Services
{
    public class FileSystemAccessService(Lazy<ISettingsService> settingsService) : IFileSystemAccessService
    {
        private Lazy<ISettingsService> _settingsService = settingsService;

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

        public string ReadAllText(string fileName)
        {
            return WindowsUse.ReadAllText(fileName);
        }

        public void WriteAllText(string fileName, string text)
        {
            WindowsUse.WriteAllText(fileName, text);
        }

        public void FilesMoveToDirectorySlowly(IEnumerable<string> filePaths, string directoryPath, Action<string, bool>? statusUpdateAction = null)
        {

                WindowsUse.FilesMoveToDirectorySlowly(filePaths, directoryPath, statusUpdateAction,
                    _settingsService.Value.CurrentSettings.CloudingAverageSpeedByKBytePerSecond,
                    _settingsService.Value.CurrentSettings.CloudingPauseBetweenFilesByms);
        }

        public void FilesMoveToDirectory(IEnumerable<string> filePaths, string directoryPath, Action<string, bool>? statusUpdateAction = null)
        {
            WindowsUse.FilesMoveToDirectory(filePaths, directoryPath, statusUpdateAction);
        }

        public void RenameFile(string path, string newName)
        { 
            var newPath = StringUtils.ReplaceFileName(path, newName);

            WindowsUse.RenameFile(path, newPath);
        }
    }
}

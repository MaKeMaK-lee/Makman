

using Makman.Middle.Entities;

namespace Makman.Middle.Services
{
    public class FileMoverService(IFileSystemAccessService fileSystemAccessService) : IFileMoverService
    {
        IFileSystemAccessService _fileSystemAccessService = fileSystemAccessService;

        public void FilesMoveToDirectory(IEnumerable<string> filePaths, CollectionDirectory directory, Action<string>? statusUpdateAction = null)
        {
            if (filePaths.Any())
            {
                if (directory.SynchronizingWithCloud == true)
                {
                    Task.Run(() =>
                    {
                        _fileSystemAccessService.FilesMoveToDirectorySlowly(
                        filePaths,
                        directory.Path,
                        statusUpdateAction);
                    });
                }
                else
                {
                    Task.Run(() =>
                    {
                        _fileSystemAccessService.FilesMoveToDirectory(
                        filePaths,
                        directory.Path,
                        statusUpdateAction);
                    });
                }
            }

        }
    }
}

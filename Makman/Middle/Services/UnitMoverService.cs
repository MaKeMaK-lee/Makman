

using Makman.Middle.Entities;

namespace Makman.Middle.Services
{
    public class UnitMoverService(IFileSystemAccessService fileSystemAccessService) : IUnitMoverService
    {
        IFileSystemAccessService _fileSystemAccessService = fileSystemAccessService;

        private bool isMoving = false;

        public void FilesMoveToDirectory(IEnumerable<Unit> units, CollectionDirectory directory, Action<string, bool>? statusUpdateAction = null)
        {
            if (!isMoving)
            {
                isMoving = true;
                if (units.Any())
                {
                    Action<IEnumerable<string>, string, Action<string, bool>?> filesMoveAction =
                        directory.SynchronizingWithCloud == true ?
                        _fileSystemAccessService.FilesMoveToDirectorySlowly :
                        _fileSystemAccessService.FilesMoveToDirectory;
                     
                    filesMoveAction(
                        units.Select(u => u.FullFileName),
                        directory.Path,
                        statusUpdateAction); 
                }
                isMoving = false;
            }
        }
    }
}

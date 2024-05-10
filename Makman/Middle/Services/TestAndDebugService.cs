using Makman.Middle.EntityManagementServices; 

namespace Makman.Middle.Services
{
    public class TestAndDebugService(IFileSystemAccessService fileSystemAccessService, IUnitManagementService unitManagementService) : ITestAndDebugService
    {
        private readonly IFileSystemAccessService _fileSystemAccessService = fileSystemAccessService;
        private readonly IUnitManagementService _unitManagementService = unitManagementService;

        public void DatabaseFill()
        {
            var chooseResult = _fileSystemAccessService.ChooseDirectory();
            if (chooseResult == null)
            {
                return;
            }
            _unitManagementService.AddUnitsFromFilesOfFolderAndSyncToDatabase(chooseResult);
        }
    }
}

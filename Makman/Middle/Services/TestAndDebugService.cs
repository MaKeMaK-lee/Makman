using Makman.Middle.EntityManagementServices; 

namespace Makman.Middle.Services
{
    public class TestAndDebugService(IFileSystemAccessService windowsAccessService, IUnitManagementService unitManagementService) : ITestAndDebugService
    {
        private readonly IFileSystemAccessService _windowsAccessService = windowsAccessService;
        private readonly IUnitManagementService _unitManagementService = unitManagementService;

        public void DatabaseFill()
        {
            var chooseResult = _windowsAccessService.ChooseDirectory();
            if (chooseResult == null)
            {
                return;
            }
            _unitManagementService.AddUnitsFromFilesOfFolderAndSyncToDatabase(chooseResult);
        }
    }
}

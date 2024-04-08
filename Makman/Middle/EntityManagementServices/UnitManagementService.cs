 
using Makman.Middle.Entities;
using Makman.Middle.Services;
using System.IO;

namespace Makman.Middle.EntityManagementServices
{
    public class UnitManagementService(ICollectionDatabaseService collectionDatabaseService, IWindowsAccessService windowsAccessService) : IUnitManagementService
    {
        readonly ICollectionDatabaseService _collectionDatabaseService = collectionDatabaseService;
        readonly IWindowsAccessService _windowsAccessService = windowsAccessService;
         
        public int AddUnitsFromFilesOfFolderAndSyncToDatabase(string directoryPath, bool allowSubDirectories = true)
        {
            IEnumerable<string> fullFileNamesToAdd = [];
            if (allowSubDirectories)
                fullFileNamesToAdd = _windowsAccessService.GetFilesFromDirectoryAndChilderns(directoryPath);
            else
                fullFileNamesToAdd = _windowsAccessService.GetFilesFromDirectory(directoryPath);

            var units = CreateMany(fullFileNamesToAdd.Where(name => !_collectionDatabaseService.GetUnits().Where(unit => unit.FullFileName == name).Any()));
            _collectionDatabaseService.Add(units); 
            _collectionDatabaseService.Save(); 

            return fullFileNamesToAdd.Count() - units.Count();
        } 

        public IEnumerable<Unit> CreateMany(IEnumerable<string> fullFileNames)
        {
            List<Unit> units = new(fullFileNames.Count());
            foreach (string fullFileName in fullFileNames)
            {
                units.Add(new Unit()
                {
                    Id = Guid.NewGuid(),
                    FullFileName = fullFileName,
                    FileName = fullFileName.Split("\\").Last(),
                    FileSize = new FileInfo(fullFileName).Length,
                    ChildUnits = [],
                    Tags = []
                });
            }
            return units;
        }

    }
}

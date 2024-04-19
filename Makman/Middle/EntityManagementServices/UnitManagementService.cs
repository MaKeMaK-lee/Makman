
using Makman.Middle.Entities;
using Makman.Middle.Services;
using System.IO;

namespace Makman.Middle.EntityManagementServices
{
    public class UnitManagementService(ICollectionDatabaseService collectionDatabaseService,
        IWindowsAccessService windowsAccessService, IBunchManagementService bunchManagementService) : IUnitManagementService
    {
        readonly ICollectionDatabaseService _collectionDatabaseService = collectionDatabaseService;
        readonly IWindowsAccessService _windowsAccessService = windowsAccessService;
        readonly IBunchManagementService _bunchManagementService = bunchManagementService;

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

        public Bunch? GetBunchOf(IEnumerable<Unit> units)
        {
            return units.FirstOrDefault(u => u?.Bunch != null, null)?.Bunch;
        }

        public bool IsSameOrNullBunch(IEnumerable<Unit>? units)
        {
            if (units?.Count() > 0)
            {
#pragma warning disable CS8602 // Разыменование вероятной пустой ссылки.
                var first = units.FirstOrDefault(u => u.Bunch != null, null);
#pragma warning restore CS8602 // Разыменование вероятной пустой ссылки.
                if (first == null)
                    return true;
                return units.Where(u => u.Bunch != null).All(u => u.Bunch == first.Bunch);
            }
            return true;
        }

        public void BindToBunch(IEnumerable<Unit> bindingUnits, Bunch? nullableBunch)
        {
            Bunch bunch = nullableBunch ?? _bunchManagementService.AddNew();

            foreach (var item in bindingUnits.Where(i => i.Bunch != bunch))
            {
                item.Bunch = bunch;//TODO 0 warning dbidect
            }
            _collectionDatabaseService.Save();
        }


        public bool TryBindToBunch(IEnumerable<Unit>? bindingUnits, IEnumerable<Unit>? exampleUnits, bool includeExamples = false)
        {
            if (bindingUnits == null || exampleUnits == null)
                return false;
            if (!bindingUnits.Any() || !exampleUnits.Any())
                return false;
            if (!IsSameOrNullBunch(exampleUnits))
                return false;

            if (includeExamples)
                BindToBunch(bindingUnits.Concat(exampleUnits), GetBunchOf(exampleUnits));
            else
                BindToBunch(bindingUnits, GetBunchOf(exampleUnits));

            return true;

        }
    }
}

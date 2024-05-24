
using Makman.Middle.Entities;
using Makman.Middle.Services;
using Makman.Middle.Utilities;
using Makman.Visual.Localization;
using System.Collections.ObjectModel;

namespace Makman.Middle.EntityManagementServices
{
    public class UnitManagementService(ICollectionDatabaseService collectionDatabaseService,
        IFileSystemAccessService fileSystemAccessService, IBunchManagementService bunchManagementService,
        IUnitMoverService unitMoverService) : IUnitManagementService
    {

        readonly ICollectionDatabaseService _collectionDatabaseService = collectionDatabaseService;
        readonly IFileSystemAccessService _fileSystemAccessService = fileSystemAccessService;
        readonly IBunchManagementService _bunchManagementService = bunchManagementService;
        readonly IUnitMoverService _unitMoverService = unitMoverService;

        public int AddUnitsFromFilesOfFolderAndSyncToDatabase(string directoryPath, bool allowSubDirectories = true)
        {
            IEnumerable<string> fullFileNamesToAdd = [];
            if (allowSubDirectories)
                fullFileNamesToAdd = _fileSystemAccessService.GetFilesFromDirectoryAndChilderns(directoryPath);
            else
                fullFileNamesToAdd = _fileSystemAccessService.GetFilesFromDirectory(directoryPath);

            var units = CreateMany(fullFileNamesToAdd.Where(name => !_collectionDatabaseService.GetUnits()
                .Where(unit => unit.FullFileName == name).Any()));

            AddUnitsToDatabase(units);

            return fullFileNamesToAdd.Count() - units.Count();
        }

        public void AddUnitsToDatabase(IEnumerable<Unit> units)
        {
            _collectionDatabaseService.Add(units);
            _collectionDatabaseService.Save();
        }

        public IEnumerable<Unit> CreateMany(IEnumerable<string> fullFileNames, IEnumerable<Tag>? tags = null, bool bindToBunch = false)
        {
            List<Unit> units = new(fullFileNames.Count());
            var nowDateTimeForThisCreates = DateTime.Now;

            foreach (string fullFileName in fullFileNames)
            {
                units.Add(new Unit()
                {
                    Id = Guid.NewGuid(),
                    FullFileName = fullFileName,
                    FileName = fullFileName.Split("\\").Last(),
                    FileSize = _fileSystemAccessService.GetFileSize(fullFileName),
                    TimeAddedToCollection = nowDateTimeForThisCreates,
                    TimeLastWrite = _fileSystemAccessService.GetFileLastWriteTime(fullFileName),
                    ChildUnits = [],
                    Tags = new ObservableCollection<Tag>(tags ?? [])
                });
            }
            if (bindToBunch)
            {
                BindToBunch(units, null);
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

        public void BindToBunch(IEnumerable<Unit> bindingUnits, Bunch? nullableBunch = null)
        {
            Bunch bunch = nullableBunch ?? _bunchManagementService.AddNew();

            foreach (var item in bindingUnits.Where(i => i.Bunch != bunch))
            {
                item.Bunch = bunch;
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

        public void UnbindTag(Unit unit, Tag removingTag)
        {
            unit.Tags.Remove(removingTag);
        }

        public void TryUnbindTag(IEnumerable<Unit>? units, Tag? removingTag)
        {
            if (units == null || removingTag == null)
                return;
            if (!units.Any())
                return;

            foreach (var item in units)
            {
                UnbindTag(item, removingTag);
            }
        }

        public void BindTag(Unit unit, Tag bindingTag)
        {
            unit.Tags.Add(bindingTag);
        }

        public void TryBindTag(IEnumerable<Unit>? units, Tag? bindingTag)
        {
            if (units == null || bindingTag == null)
                return;
            if (!units.Any())
                return;

            foreach (var item in units)
            {
                if (!item.Tags.Contains(bindingTag))
                    BindTag(item, bindingTag);
            }
        }

        public bool TryRename(Unit unit, string newFileName)
        {
            try
            {
                _fileSystemAccessService.RenameFile(unit.FullFileName, newFileName);
            }
            catch (Exception)
            {
                return false;
            }
            unit.FullFileName = StringUtils.ReplaceFileName(unit.FullFileName, newFileName);
            ActualizeFileName(unit);
            return true;
        }

        public void Rename(Unit unit, string newFileName)
        {
            _fileSystemAccessService.RenameFile(unit.FullFileName, newFileName);
            unit.FullFileName = StringUtils.ReplaceFileName(unit.FullFileName, newFileName);
            ActualizeFileName(unit);
        }

        public void StartMovingUnitsToDirectory(IEnumerable<Unit> units, CollectionDirectory directory, Action<string, bool>? statusUpdateAction)
        {
            Task.Run(() =>
            {
                _unitMoverService.FilesMoveToDirectory(units, directory, statusUpdateAction);
            }).ContinueWith(t =>
            {
                foreach (var u in units)
                {
                    u.FullFileName = StringUtils.ReplaceFileDirectory(u.FullFileName, directory.Path);
                }
            });
        }

        public void AddNamePostfix(IEnumerable<Unit> postfixingUnits)
        {
            do
            {
                foreach (var u in postfixingUnits)
                {
                    string newFileName = u.FullFileName;
                    do
                    {
                        newFileName = StringUtils.AddPostfixToFileName(
                            newFileName,
                            " - " + UIText.u_filesystem_copy).GetFileName();
                    } while (!TryRename(u, newFileName));
                }
            } while (_collectionDatabaseService.GetUnitsDuplicatedByNames(postfixingUnits).Any());
        }

        public void ActualizeFileName(Unit unit)
        {
            unit.FileName = unit.FullFileName.GetFileName();
        }
    }
}

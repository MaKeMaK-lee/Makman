﻿
using Makman.Middle.Entities;
using Makman.Middle.Services;
using System.Diagnostics;
using System.IO;

namespace Makman.Middle.EntityManagementServices
{
    public class UnitManagementService(ICollectionDatabaseService collectionDatabaseService,
        IFileSystemAccessService windowsAccessService, IBunchManagementService bunchManagementService) : IUnitManagementService
    {
        readonly ICollectionDatabaseService _collectionDatabaseService = collectionDatabaseService;
        readonly IFileSystemAccessService _windowsAccessService = windowsAccessService;
        readonly IBunchManagementService _bunchManagementService = bunchManagementService;

        public int AddUnitsFromFilesOfFolderAndSyncToDatabase(string directoryPath, bool allowSubDirectories = true)
        {
            IEnumerable<string> fullFileNamesToAdd = [];
            if (allowSubDirectories)
                fullFileNamesToAdd = _windowsAccessService.GetFilesFromDirectoryAndChilderns(directoryPath);
            else
                fullFileNamesToAdd = _windowsAccessService.GetFilesFromDirectory(directoryPath);

            var units = CreateMany(fullFileNamesToAdd.Where(name => !_collectionDatabaseService.GetUnits()
                .Where(unit => unit.FullFileName == name).Any()));

            _collectionDatabaseService.Add(units);
            _collectionDatabaseService.Save();

            return fullFileNamesToAdd.Count() - units.Count();
        }

        public IEnumerable<Unit> CreateMany(IEnumerable<string> fullFileNames)
        {
            List<Unit> units = new(fullFileNames.Count());
            var now = DateTime.Now;

            foreach (string fullFileName in fullFileNames)
            {
                units.Add(new Unit()
                {
                    Id = Guid.NewGuid(),
                    FullFileName = fullFileName,
                    FileName = fullFileName.Split("\\").Last(),
                    FileSize = new FileInfo(fullFileName).Length,
                    TimeAddedToCollection = now,
                    TimeLastWrite = new FileInfo(fullFileName).LastWriteTime,
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
    }
}

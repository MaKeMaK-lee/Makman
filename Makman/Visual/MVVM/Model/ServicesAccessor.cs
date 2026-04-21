using Makman_Entities;
using Makman_Entities.Settings;
using Makman_Middle.Services;

namespace Makman.Visual.MVVM.Model
{
    public class ServicesAccessor(
        ITestAndDebugService testAndDebugService,
        ICollectionDatabaseService collectionDatabaseService,
        IFileSystemAccessService fileSystemAccessService,
        ISettingsService settingsService) : IServicesAccessor
    {
        private readonly ITestAndDebugService _testAndDebugService = testAndDebugService;
        private readonly ICollectionDatabaseService _collectionDatabaseService = collectionDatabaseService;
        private readonly IFileSystemAccessService _fileSystemAccessService = fileSystemAccessService;
        private readonly ISettingsService _settingsService = settingsService;

        public void DatabaseFill()
        {
            _testAndDebugService.DatabaseFill();
        }

        public IEnumerable<CollectionDirectory> GetDirectories()
        {
            return _collectionDatabaseService.GetCollectionDirectories();
        }

        public IEnumerable<Tag> GetTags()
        {
            return _collectionDatabaseService.GetTags();
        }

        public IEnumerable<TagCategory> GetTagCategories()
        {
            return _collectionDatabaseService.GetTagCategories();
        }

        public IEnumerable<IEnumerable<Unit>> FindUnitsWhereSomeLooksLikeDuplicate()
        {
            return _collectionDatabaseService.FindUnitsWhereSomeLooksLikeDuplicate();
        }

        public IEnumerable<IEnumerable<Unit>> GetUnitsDuplicatedByNames(IEnumerable<Unit> newUnits)
        {
            return _collectionDatabaseService.GetUnitsDuplicatedByNames(newUnits);
        }

        public IEnumerable<string> GetAllFiles(IEnumerable<string> paths)
        {
            return _fileSystemAccessService.GetAllFiles(paths);
        }

        public void ViewInExplorer(string path)
        {
            _fileSystemAccessService.ViewInExplorer(path);
        }

        public IEnumerable<Unit> GetUnits()
        {
            return _collectionDatabaseService.GetUnits();
        }

        public bool SaveDatabase()
        {
            return _collectionDatabaseService.Save();
        }

        public bool SaveSettings()
        {
            return _settingsService.Save();
        }

        public Settings GetSettings()
        {
            return _settingsService.CurrentSettings;
        }
    }
}

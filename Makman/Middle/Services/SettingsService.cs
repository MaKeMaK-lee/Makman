
using Makman.Middle.Core;
using Makman.Middle.Entities;
using Makman.Middle.Entities.Json; 
using System.Text.Json;

namespace Makman.Middle.Services
{
    public class SettingsService : ObservableObject, ISettingsService
    {
        IFileSystemAccessService _fileSystemAccessService;
        ICollectionDatabaseService _collectionDatabaseService;

        private const string settingsFileName = "Settings.json";

        private Settings CurrentSettings { get; set; }

        public int MainWindowPositionX
        {
            get => CurrentSettings.MainWindowPositionX;
            set
            {
                CurrentSettings.MainWindowPositionX = value;
                OnPropertyChanged(nameof(MainWindowPositionX));
            }
        }
        public int MainWindowPositionY
        {
            get => CurrentSettings.MainWindowPositionY;
            set
            {
                CurrentSettings.MainWindowPositionY = value;
                OnPropertyChanged(nameof(MainWindowPositionY));
            }
        }
        public int MainWindowWidth
        {
            get => CurrentSettings.MainWindowWidth;
            set
            {
                CurrentSettings.MainWindowWidth = value;
                OnPropertyChanged(nameof(MainWindowWidth));
            }
        }
        public int MainWindowHeight
        {
            get => CurrentSettings.MainWindowHeight;
            set
            {
                CurrentSettings.MainWindowHeight = value;
                OnPropertyChanged(nameof(MainWindowHeight));
            }
        }
        public string MainDirectoryPath
        {
            get => CurrentSettings.MainDirectoryPath;
            set
            {
                CurrentSettings.MainDirectoryPath = value;
                OnPropertyChanged(nameof(MainDirectoryPath));
            }
        }
        public string TagCategoryNameForBindTagToDirectory
        {
            get => CurrentSettings.TagCategoryNameForBindTagToDirectory;
            set
            {
                CurrentSettings.TagCategoryNameForBindTagToDirectory = value;
                OnPropertyChanged(nameof(TagCategoryNameForBindTagToDirectory));
            }
        }
        public bool DefaultCollectionDirectoryAutoScanning
        {
            get => CurrentSettings.DefaultCollectionDirectoryAutoScanning;
            set
            {
                CurrentSettings.DefaultCollectionDirectoryAutoScanning = value;
                OnPropertyChanged(nameof(DefaultCollectionDirectoryAutoScanning));
            }
        }
        public bool DefaultCollectionDirectorySynchronizingWithCloud
        {
            get => CurrentSettings.DefaultCollectionDirectorySynchronizingWithCloud;
            set
            {
                CurrentSettings.DefaultCollectionDirectorySynchronizingWithCloud = value;
                OnPropertyChanged(nameof(DefaultCollectionDirectorySynchronizingWithCloud));
            }
        }
        public bool DefaultTryMoveFilesByDirectoryTagcategoryNameOnAdding
        {
            get => CurrentSettings.DefaultTryMoveFilesByDirectoryTagcategoryNameOnAdding;
            set
            {
                CurrentSettings.DefaultTryMoveFilesByDirectoryTagcategoryNameOnAdding = value;
                OnPropertyChanged(nameof(DefaultTryMoveFilesByDirectoryTagcategoryNameOnAdding));
            }
        }

        private CollectionDirectory? defaultDirectoryPathToMoveOnAdding = null;
        public CollectionDirectory? DefaultDirectoryPathToMoveOnAdding
        {
            get
            {
                if (defaultDirectoryPathToMoveOnAdding != null)
                    return defaultDirectoryPathToMoveOnAdding;

                if (!_collectionDatabaseService.IsContainCollectionDirectoryWithPath(CurrentSettings.DefaultDirectoryPathToMoveOnAdding))
                {
                    defaultDirectoryPathToMoveOnAdding = null;
                    return defaultDirectoryPathToMoveOnAdding;
                }

                defaultDirectoryPathToMoveOnAdding = _collectionDatabaseService.GetCollectionDirectoryByPath(CurrentSettings.DefaultDirectoryPathToMoveOnAdding);
                return defaultDirectoryPathToMoveOnAdding;
            }
            set
            {
                defaultDirectoryPathToMoveOnAdding = value;
                OnPropertyChanged(nameof(DefaultDirectoryPathToMoveOnAdding));
            }
        }
        private IEnumerable<Tag>? defaultTagsOnAdding = null;
        public IEnumerable<Tag> DefaultTagsOnAdding
        {
            get
            {
                if (defaultTagsOnAdding != null)
                    return defaultTagsOnAdding;

                defaultTagsOnAdding = _collectionDatabaseService.GetTagsByNamesLower(CurrentSettings.DefaultTagsOnAdding);
                return defaultTagsOnAdding;
            }
        }

        private bool TryLoadCurrentSettings()
        {
            try
            {
                if (_fileSystemAccessService.FileExists(settingsFileName))
                {
                    string jsonString = _fileSystemAccessService.ReadAllText(settingsFileName);
                    var settings = JsonSerializer.Deserialize<Settings>(jsonString);
                    if (settings != null)
                    {
                        CurrentSettings = settings;
                        return true;
                    }
                }
            }
            catch (Exception)
            {

            }
            return false;
        }

        private void SynchronizeNonSyncronizedCurrentSettings()
        {
            CurrentSettings.DefaultTagsOnAdding = DefaultTagsOnAdding.Select(t => t.Name).ToList();

            if (DefaultDirectoryPathToMoveOnAdding?.Path != null)
                CurrentSettings.DefaultDirectoryPathToMoveOnAdding = DefaultDirectoryPathToMoveOnAdding.Path;
            else
                CurrentSettings.DefaultDirectoryPathToMoveOnAdding = "";
        }

        public bool Save()
        {
            SynchronizeNonSyncronizedCurrentSettings();

            try
            {
                string fileName = settingsFileName;
                string jsonString = JsonSerializer.Serialize(CurrentSettings, options: new()
                {
                    IgnoreReadOnlyProperties = true,
                    WriteIndented = true
                });
                _fileSystemAccessService.WriteAllText(fileName, jsonString);
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        public SettingsService(IFileSystemAccessService fileSystemAccessService, ICollectionDatabaseService collectionDatabaseService)
        {
            _fileSystemAccessService = fileSystemAccessService;
            _collectionDatabaseService = collectionDatabaseService;

            if (!TryLoadCurrentSettings())
                SetDefaultSettings();

        }

        public void SetDefaultSettings()
        {
            CurrentSettings = GetDefaultSettingsCopy();
        }

        public Settings GetDefaultSettingsCopy()
        {
            return new Settings
            {
                MainWindowPositionX = -1,
                MainWindowPositionY = -1,
                MainWindowWidth = 920,
                MainWindowHeight = 600,

                MainDirectoryPath = "",
                TagCategoryNameForBindTagToDirectory = "",

                DefaultCollectionDirectoryAutoScanning = true,
                DefaultCollectionDirectorySynchronizingWithCloud = true,

                DefaultTryMoveFilesByDirectoryTagcategoryNameOnAdding = false,
                DefaultDirectoryPathToMoveOnAdding = "",
                DefaultTagsOnAdding = []
            };
        }
    }
}
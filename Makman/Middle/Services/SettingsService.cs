
using Makman.Middle.Core;
using Makman.Middle.Entities;
using Makman.Middle.Entities.Settings;
using System.Collections.ObjectModel;
using System.Text.Json;

namespace Makman.Middle.Services
{
    public class SettingsService : ObservableObject, ISettingsService
    {
        IFileSystemAccessService _fileSystemAccessService;
        ICollectionDatabaseService _collectionDatabaseService;

        private const string settingsFileName = "Settings.json";

        public Settings CurrentSettings { get; set; }

        private SettingsForJsonStoring SettingsToSettingsForJsonStoring(Settings settings)
        {
            return new SettingsForJsonStoring
            {
                MainWindowPositionX = settings.MainWindowPositionX,
                MainWindowPositionY = settings.MainWindowPositionY,
                MainWindowWidth = settings.MainWindowWidth,
                MainWindowHeight = settings.MainWindowHeight,

                CloudingPauseBetweenFilesByms = settings.CloudingPauseBetweenFilesByms,
                CloudingAverageSpeedByKBytePerSecond = settings.CloudingAverageSpeedByKBytePerSecond,
                MainDirectoryPath = settings.MainDirectory?.Path ?? "",
                TagCategoryNameForBindTagToDirectory = settings.TagCategoryForBindTagToDirectory?.Name ?? "",

                CollectionDirectoryAutoScanningDefaultValue = settings.CollectionDirectoryAutoScanningDefaultValue,
                CollectionDirectorySynchronizingWithCloudDefaultValue = settings.CollectionDirectorySynchronizingWithCloudDefaultValue,

                DefaultDirectoryPathToMoveOnAdding = settings.DefaultTargetDirectoryToMoveOnAdding?.Path ?? "",
                DefaultTagsOnAdding = settings.TagsOnAddingUnits?.Select(t => t.Name).ToList() ?? [],

                TryMoveFilesOnAdding = settings.TryMoveFilesOnAdding,
                AddTagsOnAddingUnits = settings.AddTagsOnAddingUnits,
                TryMoveFilesByDirectoryTagcategoryNameOnAdding = settings.TryMoveFilesByDirectoryTagcategoryNameOnAdding,
                ToggleBunchingOnAddingUnits = settings.ToggleBunchingOnAddingUnits,
            };
        }

        private Settings SettingsForJsonStoringToSettings(SettingsForJsonStoring settingsJson)
        {
            CollectionDirectory? mainDirectory;
            CollectionDirectory? defaultDirectoryToMoveOnAdding;
            TagCategory? tagCategoryForBindTagToDirectory;
            ObservableCollection<Tag>? defaultTagsOnAdding;

            if (!_collectionDatabaseService.IsContainCollectionDirectoryWithPath(settingsJson.MainDirectoryPath))
                mainDirectory = null;
            else
                mainDirectory = _collectionDatabaseService.GetCollectionDirectory(settingsJson.MainDirectoryPath);

            if (!_collectionDatabaseService.IsContainCollectionDirectoryWithPath(settingsJson.DefaultDirectoryPathToMoveOnAdding))
                defaultDirectoryToMoveOnAdding = null;
            else
                defaultDirectoryToMoveOnAdding = _collectionDatabaseService.GetCollectionDirectory(settingsJson.DefaultDirectoryPathToMoveOnAdding);

            if (!_collectionDatabaseService.IsContainTagCategoryWithNameLower(settingsJson.TagCategoryNameForBindTagToDirectory))
                tagCategoryForBindTagToDirectory = null;
            else
                tagCategoryForBindTagToDirectory = _collectionDatabaseService.GetTagCategoryLower(settingsJson.TagCategoryNameForBindTagToDirectory);

            defaultTagsOnAdding = new ObservableCollection<Tag>(_collectionDatabaseService.GetTagsByNamesLower(settingsJson.DefaultTagsOnAdding));


            return new Settings
            {
                MainWindowPositionX = settingsJson.MainWindowPositionX,
                MainWindowPositionY = settingsJson.MainWindowPositionY,
                MainWindowWidth = settingsJson.MainWindowWidth,
                MainWindowHeight = settingsJson.MainWindowHeight,

                CloudingPauseBetweenFilesByms = settingsJson.CloudingPauseBetweenFilesByms,
                CloudingAverageSpeedByKBytePerSecond = settingsJson.CloudingAverageSpeedByKBytePerSecond,
                MainDirectory = mainDirectory,
                TagCategoryForBindTagToDirectory = tagCategoryForBindTagToDirectory,

                CollectionDirectoryAutoScanningDefaultValue = settingsJson.CollectionDirectoryAutoScanningDefaultValue,
                CollectionDirectorySynchronizingWithCloudDefaultValue = settingsJson.CollectionDirectorySynchronizingWithCloudDefaultValue,

                TryMoveFilesOnAdding = settingsJson.TryMoveFilesOnAdding,
                AddTagsOnAddingUnits = settingsJson.AddTagsOnAddingUnits,
                TryMoveFilesByDirectoryTagcategoryNameOnAdding = settingsJson.TryMoveFilesByDirectoryTagcategoryNameOnAdding,
                DefaultTargetDirectoryToMoveOnAdding = defaultDirectoryToMoveOnAdding,
                CurrentTargetDirectoryToMoveOnAdding = defaultDirectoryToMoveOnAdding,
                TagsOnAddingUnits = defaultTagsOnAdding,
                ToggleBunchingOnAddingUnits = settingsJson.ToggleBunchingOnAddingUnits,
            };
        }


        private bool TryLoadCurrentSettings()
        {
            try
            {
                if (_fileSystemAccessService.FileExists(settingsFileName))
                {
                    string jsonString = _fileSystemAccessService.ReadAllText(settingsFileName);
                    var jsonSettings = JsonSerializer.Deserialize<SettingsForJsonStoring>(jsonString);
                    if (jsonSettings != null)
                    {
                        CurrentSettings = SettingsForJsonStoringToSettings(jsonSettings);
                        return true;
                    }
                }
            }
            catch (Exception)
            {

            }
            return false;
        }

        public bool Save()
        {
            try
            {
                string fileName = settingsFileName;
                string jsonString = JsonSerializer.Serialize(SettingsToSettingsForJsonStoring(CurrentSettings), options: new()
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

                CloudingPauseBetweenFilesByms = 1000,
                CloudingAverageSpeedByKBytePerSecond = 512,
                MainDirectory = null,
                TagCategoryForBindTagToDirectory = null,
                ToggleBunchingOnAddingUnits = false,

                CollectionDirectoryAutoScanningDefaultValue = true,
                CollectionDirectorySynchronizingWithCloudDefaultValue = true,

                TryMoveFilesOnAdding = false,
                AddTagsOnAddingUnits = false,
                TryMoveFilesByDirectoryTagcategoryNameOnAdding = false,
                DefaultTargetDirectoryToMoveOnAdding = null,
                CurrentTargetDirectoryToMoveOnAdding = null,
                TagsOnAddingUnits = []
            };
        }
    }
}
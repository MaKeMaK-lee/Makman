 

namespace Makman.Middle.Entities.Settings
{
    public class SettingsForJsonStoring
    {
        public required int MainWindowPositionX { get; set; }
        public required int MainWindowPositionY { get; set; }
        public required int MainWindowWidth { get; set; }
        public required int MainWindowHeight { get; set; }

        public required long CloudingAverageSpeedByKBytePerSecond { get; set; }
        public required int CloudingPauseBetweenFilesByms { get; set; }
        public required string MainDirectoryPath { get; set; }
        public required string TagCategoryNameForBindTagToDirectory { get; set; }

        public required bool CollectionDirectoryAutoScanningDefaultValue { get; set; }
        public required bool CollectionDirectorySynchronizingWithCloudDefaultValue { get; set; }

        public required bool TryMoveFilesByDirectoryTagcategoryNameOnAdding { get; set; }
        public required string DefaultDirectoryPathToMoveOnAdding { get; set; }
        public required List<string> DefaultTagsOnAdding { get; set; }
        public required bool ToggleBunchingOnAddingUnits { get; set; }
        public required bool AddTagsOnAddingUnits { get; set; }
        public required bool TryMoveFilesOnAdding { get; set; }

    }
}

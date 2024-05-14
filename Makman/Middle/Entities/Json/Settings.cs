
using Makman.Middle.Core;
using System.Collections.ObjectModel;
using System.Text.Json.Serialization;

namespace Makman.Middle.Entities.Json
{
    public class Settings
    {
        public required int MainWindowPositionX { get; set; }
        public required int MainWindowPositionY { get; set; }
        public required int MainWindowWidth { get; set; }
        public required int MainWindowHeight { get; set; }

        public required long CloudingAverageSpeedByKBytePerSecond { get; set; }
        public required int CloudingPauseBetweenFilesByms { get; set; }
        public required string MainDirectoryPath { get; set; }
        public required string TagCategoryNameForBindTagToDirectory { get; set; }

        public required bool DefaultCollectionDirectoryAutoScanning { get; set; }
        public required bool DefaultCollectionDirectorySynchronizingWithCloud { get; set; }

        public required bool DefaultTryMoveFilesByDirectoryTagcategoryNameOnAdding { get; set; }
        public required string DefaultDirectoryPathToMoveOnAdding { get; set; }
        public required List<string> DefaultTagsOnAdding { get; set; }
         
    }
}

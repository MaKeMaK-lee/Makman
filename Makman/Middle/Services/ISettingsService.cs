using Makman.Middle.Entities; 

namespace Makman.Middle.Services
{
    public interface ISettingsService
    {
        bool Save();

        int MainWindowPositionX { get; set; }
        int MainWindowPositionY { get; set; }
        int MainWindowWidth { get; set; }
        int MainWindowHeight { get; set; }

        string MainDirectoryPath { get; set; }
        string TagCategoryNameForBindTagToDirectory { get; set; }

        bool DefaultCollectionDirectoryAutoScanning { get; set; }
        bool DefaultCollectionDirectorySynchronizingWithCloud { get; set; }

        bool DefaultTryMoveFilesByDirectoryTagcategoryNameOnAdding { get; set; }
        CollectionDirectory? DefaultDirectoryPathToMoveOnAdding { get; set; }
        IEnumerable<Tag> DefaultTagsOnAdding { get; }

















    }
}

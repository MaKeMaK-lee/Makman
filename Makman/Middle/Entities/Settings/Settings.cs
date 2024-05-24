using Makman.Middle.Core; 
using System.Collections.ObjectModel; 

namespace Makman.Middle.Entities.Settings
{
    public class Settings : ObservableObject
    {
        private bool tryMoveFilesOnAdding;
        private bool toggleBunchingOnAddingUnits;
        private bool addTagsOnAddingUnits;
        private bool tryMoveFilesByDirectoryTagcategoryNameOnAdding;

        //MainWindow
        public required int MainWindowPositionX { get; set; }

        public required int MainWindowPositionY { get; set; }

        public required int MainWindowWidth { get; set; }

        public required int MainWindowHeight { get; set; }

        //File moving
        public required long CloudingAverageSpeedByKBytePerSecond { get; set; }

        public required int CloudingPauseBetweenFilesByms { get; set; }

        //Default CollectionDirectory props
        public required bool CollectionDirectoryAutoScanningDefaultValue { get; set; }

        public required bool CollectionDirectorySynchronizingWithCloudDefaultValue { get; set; }

        //Unit adding props 
        public required bool ToggleBunchingOnAddingUnits
        {
            get => toggleBunchingOnAddingUnits;
            set
            {
                toggleBunchingOnAddingUnits = value;
                OnPropertyChanged(nameof(ToggleBunchingOnAddingUnits));
            }
        } 

        public required bool AddTagsOnAddingUnits
        {
            get => addTagsOnAddingUnits;
            set
            {
                addTagsOnAddingUnits = value;
                OnPropertyChanged(nameof(AddTagsOnAddingUnits));
            }
        }

        public required ObservableCollection<Tag> TagsOnAddingUnits { get; set; }

        //Unit adding directory variant 1
        /// <summary>
        /// Directory where contains subdirectories with names of tags (with "TagCategoryForBindTagToDirectory" prop TagCategory) 
        /// with units putting here when adding if "TryMoveFilesByDirectoryTagcategoryNameOnAdding" prop is true
        /// </summary>
        public required CollectionDirectory? MainDirectory { get; set; }

        public required TagCategory? TagCategoryForBindTagToDirectory { get; set; } 

        public required bool TryMoveFilesByDirectoryTagcategoryNameOnAdding
        {
            get => tryMoveFilesByDirectoryTagcategoryNameOnAdding;
            set
            {
                tryMoveFilesByDirectoryTagcategoryNameOnAdding = value;
                OnPropertyChanged(nameof(TryMoveFilesByDirectoryTagcategoryNameOnAdding));
            }
        }

        //Unit adding directory variant 2 
        /// <summary>
        /// Current directory to move files on adding. Not storing and sets to "DefaultTargetDirectoryToMoveOnAdding" prop on Makman launch.
        /// </summary>
        public required CollectionDirectory? CurrentTargetDirectoryToMoveOnAdding { get; set; }

        /// <summary>
        /// Default directory to move files on adding. 
        /// </summary>
        public required CollectionDirectory? DefaultTargetDirectoryToMoveOnAdding { get; set; }
         
        public required bool TryMoveFilesOnAdding
        {
            get => tryMoveFilesOnAdding;
            set
            {
                tryMoveFilesOnAdding = value;
                OnPropertyChanged(nameof(TryMoveFilesOnAdding));
            }
        } 
    }
}

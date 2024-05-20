using Makman.Middle.Core;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Makman.Middle.Entities.Settings
{
    public class Settings : ObservableObject
    {
        //MainWindow
        public required int MainWindowPositionX { get; set; }
        public required int MainWindowPositionY { get; set; }
        public required int MainWindowWidth { get; set; }
        public required int MainWindowHeight { get; set; }

        //File moving
        public required long CloudingAverageSpeedByKBytePerSecond { get; set; }
        public required int CloudingPauseBetweenFilesByms { get; set; }

        //Default CollectionDirectory props
        public required bool DefaultCollectionDirectoryAutoScanning { get; set; }
        public required bool DefaultCollectionDirectorySynchronizingWithCloud { get; set; }

        //Default Unit props 
        public required ObservableCollection<Tag> TagsOnAdding { get; set; }

        //Adding settings variant 1
        /// <summary>
        /// Directory where contains subdirectories with names of tags (with "TagCategoryForBindTagToDirectory" prop TagCategory) 
        /// with units putting here when adding if "DefaultTryMoveFilesByDirectoryTagcategoryNameOnAdding" prop is true
        /// </summary>
        public required CollectionDirectory? MainDirectory { get; set; }
        public required TagCategory? TagCategoryForBindTagToDirectory { get; set; }
        public required bool DefaultTryMoveFilesByDirectoryTagcategoryNameOnAdding { get; set; }

        //Adding settings variant 2 
        /// <summary>
        /// Current directory to move files on adding. Not storing and sets to "DefaultDirectoryToMoveOnAdding" prop on Makman launch.
        /// </summary>
        public required CollectionDirectory? CurrentDirectoryToMoveOnAdding { get; set; }
        public required CollectionDirectory? DefaultDirectoryToMoveOnAdding { get; set; }

    }
}


using Makman.Middle.Core;
using Makman.Middle.Entities;
using Makman.Middle.Entities.Settings;
using System.ComponentModel;

namespace Makman.Visual.Components.ViewModel
{
    public class UnitsAdderMainViewModel : Core.ViewModel, IDisposable
    {
        public required Settings Settings { get; set; }

        public IEnumerable<Tag> TagsPickerList { get; set; }

        public IEnumerable<TagCategory> TagCategoryPickerList { get; set; }

        public IEnumerable<CollectionDirectory> MainDirectoryPickerList { get; set; }

        public IEnumerable<CollectionDirectory> TargetDirectoryPickerList { get; set; }

        public Tag? tagsPickerListSelectedItem { get; set; }
        public Tag? TagsPickerListSelectedItem
        {
            get => tagsPickerListSelectedItem;
            set
            {
                tagsPickerListSelectedItem = value;
                OnPropertyChanged(nameof(TagsPickerListSelectedItem));
                OnPropertyChanged(nameof(IsSelectedAnyTag));
            }
        }

        public bool IsSelectedAnyTag => TagsPickerListSelectedItem != null;

        public double BunchingSettingsGroupOpacity => Settings.ToggleBunchingOnAddingUnits ? 1 : 0.3;

        public double AddTagsSettingsGroupOpacity => Settings.AddTagsOnAddingUnits ? 1 : 0.3;

        public double MoveToMainSettingsGroupOpacity => Settings.TryMoveFilesByDirectoryTagcategoryNameOnAdding ? 1 : 0.3;

        public double MoveToTargetSettingsGroupOpacity => Settings.TryMoveFilesOnAdding ? 1 : 0.3;

        public RelayCommand TagBindCommand { get; set; }
        public RelayCommand TagUnbindCommand { get; set; }
        
        private void SetCommands()
        {
            TagBindCommand = new RelayCommand(o =>
            {
                if (TagsPickerListSelectedItem != null)
                    if (!Settings.TagsOnAddingUnits.Contains(TagsPickerListSelectedItem))
                        Settings.TagsOnAddingUnits.Add(TagsPickerListSelectedItem);
            }, o => true);
            TagUnbindCommand = new RelayCommand(o =>
            {
                if (TagsPickerListSelectedItem != null)
                    if (Settings.TagsOnAddingUnits.Contains(TagsPickerListSelectedItem))
                        Settings.TagsOnAddingUnits.Remove(TagsPickerListSelectedItem);
            }, o => true);
        }


        private Core.ViewModel? dragDropControl;
        public Core.ViewModel? DragDropControl
        {
            get => dragDropControl;
            private set
            {
                dragDropControl = value;
                OnPropertyChanged(nameof(DragDropControl));
            }
        }

        private void WhenSettingsChanged(object? o, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(Settings.ToggleBunchingOnAddingUnits):
                    OnPropertyChanged(nameof(BunchingSettingsGroupOpacity));
                    break;
                case nameof(Settings.AddTagsOnAddingUnits):
                    OnPropertyChanged(nameof(AddTagsSettingsGroupOpacity));
                    break;
                case nameof(Settings.TryMoveFilesByDirectoryTagcategoryNameOnAdding):
                    OnPropertyChanged(nameof(MoveToMainSettingsGroupOpacity));
                    break;
                case nameof(Settings.TryMoveFilesOnAdding):
                    OnPropertyChanged(nameof(MoveToTargetSettingsGroupOpacity));
                    break;
                default:
                    break;
            }
        }

        public void StartTryingUnitCreationAction(Action<string[]> receivedFilenamesAction)
        {
            Settings.PropertyChanged += WhenSettingsChanged;

            DragDropControl = new DragDropFilesReceiverViewModel()
            {
                ReturnFilenamesAction = receivedFilenamesAction
            };
        }

        public UnitsAdderMainViewModel(IEnumerable<Tag> tags, IEnumerable<TagCategory> tagCategories, IEnumerable<CollectionDirectory> directories)
        {
            TagsPickerList = tags.Where(t => true);
            TagCategoryPickerList = tagCategories.Where(t => true);
            TargetDirectoryPickerList = directories.Where(t => true);
            MainDirectoryPickerList = directories.Where(t => true);



            SetCommands();
        }

        public void Dispose()
        {
            Settings.PropertyChanged -= WhenSettingsChanged;
        }
    }
}

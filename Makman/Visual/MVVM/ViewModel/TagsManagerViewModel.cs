using Makman.Middle.Core;
using Makman.Middle.Entities;
using Makman.Middle.EntityManagementServices;
using Makman.Visual.MVVM.Model;

namespace Makman.Visual.MVVM.ViewModel
{
    public class TagsManagerViewModel : Core.ViewModel
    {
        private readonly IServicesAccessor _servicesAccessor;
        private readonly ITagManagementService _tagManagementService;

        private IEnumerable<Tag> tagCollection;
        public IEnumerable<Tag> TagCollection
        {
            get
            {
                return tagCollection;
            }
        }

        public IEnumerable<Tag>? selectedTags;
        public IEnumerable<Tag>? SelectedTags
        {
            get => selectedTags;
            set
            {
                selectedTags = value;
                if (selectedTags?.Count() == 0)
                {
                    selectedTags = null;
                }
                OnPropertyChanged(nameof(IsSelectedOnlyOneItemInTagCollection));
                OnPropertyChanged(nameof(IsSelectedAnyItemInTagCollection));
            }
        }

        //private Tag selectedTag;
        //public Tag SelectedTag
        //{
        //    get => selectedTag;
        //    set
        //    {
        //        selectedTag = value;
        //        OnPropertyChanged(nameof(IsSelectedAnyItemInTagCollection));
        //    }
        //}

        public bool IsSelectedOnlyOneItemInTagCollection
        {
            get
            {
                if (SelectedTags == null)
                    return false;
                if (SelectedTags.Count() == 1)
                    return true;
                return false;

            }
        }
        public bool IsSelectedAnyItemInTagCollection
        {
            get
            {
                if (SelectedTags == null)
                    return false;
                else
                    return true;
            }
        }
        public RelayCommand DataGridSelectionChangedCommand { get; set; }
        public RelayCommand RemoveTagCommand { get; set; } 
        public RelayCommand AddTagCommand { get; set; }
        private void SetCommands()
        {
            AddTagCommand = new RelayCommand(o =>
            {
                _tagManagementService.AddNew();
            }, o => true); 
            RemoveTagCommand = new RelayCommand(o =>
            {
                if (SelectedTags != null)
                    _tagManagementService.Remove(SelectedTags.ToList());
            }, o => true);
            DataGridSelectionChangedCommand = new RelayCommand(o =>
            {
                if (o == null)
                    SelectedTags = null;
                else
                {
                    SelectedTags = ((IEnumerable<object>)o).Cast<Tag>();
                }
            }, o => true);
        }


        public TagsManagerViewModel(IServicesAccessor servicesAccessor, ITagManagementService tagManagementService)
        {
            _servicesAccessor = servicesAccessor;
            _tagManagementService = tagManagementService;

            SetCommands();

            tagCollection = _servicesAccessor.GetTags();
            //foreach (var item in TagCollection)
            //{
            //    item.PickTagCommand = new RelayCommand(item.PickTagCommandAction, o => true);
            //};

        }
    }
}

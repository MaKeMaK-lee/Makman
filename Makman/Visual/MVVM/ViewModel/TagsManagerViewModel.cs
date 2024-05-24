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

        private IEnumerable<TagCategory> tagCategoriesCollection;
        public IEnumerable<TagCategory> TagCategoriesCollection
        {
            get
            {
                return tagCategoriesCollection;
            }
        }

        private IEnumerable<Tag> tagCollection;
        public IEnumerable<Tag> TagCollection
        {
            get
            {
                return tagCollection;
            }
        }

        public TagCategory? selectedTagCategory;
        public TagCategory? SelectedTagCategory
        {
            get => selectedTagCategory;
            set
            {
                if (value != selectedTagCategory)
                {
                    selectedTagCategory = value;
                    OnPropertyChanged(nameof(SelectedTagCategory));
                    if (SelectedTags != null && SelectedTagCategory != null)
                        _tagManagementService.SetCategory(SelectedTags, SelectedTagCategory);
                }

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


                //Refresh SelectedTagCategory
                if (selectedTags?.Count() > 0)
                {
                    if (_tagManagementService.IsSameOrNullCategory(selectedTags))
                    {
                        SelectedTagCategory = selectedTags.First().Category;
                    }
                    else
                    {
                        SelectedTagCategory = null;
                    }
                }
                else
                {
                    SelectedTagCategory = null;
                }
            }
        }

        public string filterText;
        public string FilterText
        {
            get => filterText;
            set
            {
                filterText = value;
            }
        }

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

        public RelayCommand ComboBoxSelectionChangedCommand { get; set; }
        public RelayCommand FilterTextBoxTextChangedCommand { get; set; }
        public RelayCommand DataGridSelectionChangedCommand { get; set; }
        public RelayCommand RemoveTagCommand { get; set; }
        public RelayCommand AddTagCommand { get; set; }

        private void SetCommands()
        {
            AddTagCommand = new RelayCommand(o =>
            {
                if (!string.IsNullOrEmpty(FilterText))
                    _tagManagementService.AddNew(FilterText);
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
            ComboBoxSelectionChangedCommand = new RelayCommand(o =>
            {

            }, o => true);
        }

        public TagsManagerViewModel(IServicesAccessor servicesAccessor, ITagManagementService tagManagementService)
        {
            _servicesAccessor = servicesAccessor;
            _tagManagementService = tagManagementService;

            SetCommands();

            tagCollection = _servicesAccessor.GetTags();
            tagCategoriesCollection = _servicesAccessor.GetTagCategories();

        }
    }
}

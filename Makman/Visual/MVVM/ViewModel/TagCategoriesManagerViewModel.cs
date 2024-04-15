using Makman.Middle.Core;
using Makman.Middle.Entities;
using Makman.Middle.EntityManagementServices;
using Makman.Visual.MVVM.Model;

namespace Makman.Visual.MVVM.ViewModel
{
    public class TagCategoriesManagerViewModel : Core.ViewModel
    {
        private readonly IServicesAccessor _servicesAccessor;
        private readonly ITagCategoryManagementService _tagCategoryManagementService;

        private IEnumerable<TagCategory> tagCategoryCollection;
        public IEnumerable<TagCategory> TagCategoryCollection
        {
            get
            {
                return tagCategoryCollection;
            }
        }

        public IEnumerable<TagCategory>? selectedTagCategories;
        public IEnumerable<TagCategory>? SelectedTagCategories
        {
            get => selectedTagCategories;
            set
            {
                selectedTagCategories = value;
                if (selectedTagCategories?.Count() == 0)
                {
                    selectedTagCategories = null;
                }
                OnPropertyChanged(nameof(IsSelectedOnlyOneItemInTagCategoryCollection));
                OnPropertyChanged(nameof(IsSelectedAnyItemInTagCategoryCollection));
            }
        }
        public string filterText;
        public string FilterText
        {
            get => filterText;//TODO 201 filtering
            set
            {
                filterText = value;
            }
        }

        public bool IsSelectedOnlyOneItemInTagCategoryCollection
        {
            get
            {
                if (SelectedTagCategories == null)
                    return false;
                if (SelectedTagCategories.Count() == 1)
                    return true;
                return false;

            }
        }
        public bool IsSelectedAnyItemInTagCategoryCollection
        {
            get
            {
                if (SelectedTagCategories == null)
                    return false;
                else
                    return true;
            }
        }
        public RelayCommand FilterTextBoxTextChangedCommand { get; set; }
        public RelayCommand DataGridSelectionChangedCommand { get; set; }
        public RelayCommand RemoveTagCategoryCommand { get; set; }
        public RelayCommand AddTagCategoryCommand { get; set; }
        private void SetCommands()
        {
            AddTagCategoryCommand = new RelayCommand(o =>
            {
                if (string.IsNullOrEmpty(FilterText))
                    _tagCategoryManagementService.AddNew(FilterText);
            }, o => true);
            RemoveTagCategoryCommand = new RelayCommand(o =>
            {
                if (SelectedTagCategories != null)
                    _tagCategoryManagementService.Remove(SelectedTagCategories.ToList());
            }, o => true);
            DataGridSelectionChangedCommand = new RelayCommand(o =>
            {
                if (o == null)
                    SelectedTagCategories = null;
                else
                {
                    SelectedTagCategories = ((IEnumerable<object>)o).Cast<TagCategory>();
                }
            }, o => true);
        }


        public TagCategoriesManagerViewModel(IServicesAccessor servicesAccessor, ITagCategoryManagementService tagCategoryManagementService)
        {
            _servicesAccessor = servicesAccessor;
            _tagCategoryManagementService = tagCategoryManagementService;

            SetCommands();

            tagCategoryCollection = _servicesAccessor.GetTagCategories();
            //foreach (var item in TagCategoryCollection)
            //{
            //    item.PickTagCategoryCommand = new RelayCommand(item.PickTagCategoryCommandAction, o => true);
            //};

        }
    }
}

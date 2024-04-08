using Makman.Middle.Core;
using Makman.Middle.Entities;
using Makman.Middle.EntityManagementServices;
using Makman.Visual.MVVM.Model;

namespace Makman.Visual.MVVM.ViewModel
{
    public class DirectoriesManagerViewModel : Core.ViewModel
    {
        private readonly IServicesAccessor _servicesAccessor;
        private readonly ICollectionDirectoryManagementService _collectionDirectoryManagementService;

        private IEnumerable<CollectionDirectory> directoryCollection;
        public IEnumerable<CollectionDirectory> DirectoryCollection
        {
            get
            {
                return directoryCollection;
            }
        }

        public IEnumerable<CollectionDirectory>? selectedDirectories;
        public IEnumerable<CollectionDirectory>? SelectedDirectories
        {
            get => selectedDirectories;
            set
            {
                selectedDirectories = value;
                if (selectedDirectories?.Count() == 0)
                {
                    selectedDirectories = null;
                }
                OnPropertyChanged(nameof(IsSelectedOnlyOneItemInDirectoryCollection));
                OnPropertyChanged(nameof(IsSelectedAnyItemInDirectoryCollection));
            }
        }

        //private CollectionDirectory selectedDirectory;
        //public CollectionDirectory SelectedDirectory
        //{
        //    get => selectedDirectory;
        //    set
        //    {
        //        selectedDirectory = value;
        //        OnPropertyChanged(nameof(IsSelectedAnyItemInDirectoryCollection));
        //    }
        //}

        public bool IsSelectedOnlyOneItemInDirectoryCollection
        {
            get
            {
                if (SelectedDirectories == null)
                    return false;
                if (SelectedDirectories.Count() == 1)
                    return true;
                return false;

            }
        }
        public bool IsSelectedAnyItemInDirectoryCollection
        {
            get
            {
                if (SelectedDirectories == null)
                    return false;
                else
                    return true;
            }
        }
        public RelayCommand DataGridSelectionChangedCommand { get; set; }
        public RelayCommand RemoveDirectoryCommand { get; set; }
        public RelayCommand ChangeDirectoryPathCommand { get; set; }
        public RelayCommand AddDirectoryCommand { get; set; }
        private void SetCommands()
        {
            AddDirectoryCommand = new RelayCommand(o =>
            {
                _collectionDirectoryManagementService.AddNew();
            }, o => true);
            ChangeDirectoryPathCommand = new RelayCommand(o =>
            {
                if (SelectedDirectories != null)
                    _collectionDirectoryManagementService.PickNewPath(SelectedDirectories.First());
            }, o => true);
            RemoveDirectoryCommand = new RelayCommand(o =>
            {
                if (SelectedDirectories != null)
                    _collectionDirectoryManagementService.Remove(SelectedDirectories.ToList());
            }, o => true);
            DataGridSelectionChangedCommand = new RelayCommand(o =>
            {
                if (o == null)
                    SelectedDirectories = null;
                else
                {
                    SelectedDirectories = ((IEnumerable<object>)o).Cast<CollectionDirectory>();
                }
            }, o => true);
        }


        public DirectoriesManagerViewModel(IServicesAccessor servicesAccessor, ICollectionDirectoryManagementService collectionDirectoryManagementService)
        {
            _servicesAccessor = servicesAccessor;
            _collectionDirectoryManagementService = collectionDirectoryManagementService;

            SetCommands();

            directoryCollection = _servicesAccessor.GetDirectories();
            //foreach (var item in DirectoryCollection)
            //{
            //    item.PickDirectoryCommand = new RelayCommand(item.PickDirectoryCommandAction, o => true);
            //};

        }
    }
}

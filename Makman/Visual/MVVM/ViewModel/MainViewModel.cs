
using Makman.Middle.Core;
using Makman.Visual.Core;

namespace Makman.Visual.MVVM.ViewModel
{
    public class MainViewModel : Core.ViewModel
    {

        private INavigation _navigation;


        public INavigation Navigation
        {
            get => _navigation;
            set
            {
                _navigation = value;
                OnPropertyChanged();
            }
        }

        public RelayCommand NavigateToHomeCommand { get; set; }
        public RelayCommand NavigateToUnitsAdderCommand { get; set; }
        public RelayCommand NavigateToDiscoveryCommand { get; set; }
        public RelayCommand NavigateToCollectionCommand { get; set; }
        public RelayCommand NavigateToTagsManagerCommand { get; set; }
        public RelayCommand NavigateToTagCategoriesManagerCommand { get; set; }
        public RelayCommand NavigateToDirectoriesManagerCommand { get; set; }
        public RelayCommand NavigateToUnitsManagerCommand { get; set; }

        public MainViewModel(INavigation navigationService)
        {
            Navigation = navigationService;
            NavigateToHomeCommand = new RelayCommand(o =>
            {
                Navigation.NavigateTo<HomeViewModel>();
            }, o => true);
            NavigateToUnitsAdderCommand = new RelayCommand(o =>
            {
                Navigation.NavigateTo<UnitsAdderViewModel>();
            }, o => true);
            NavigateToDiscoveryCommand = new RelayCommand(o =>
            {
                Navigation.NavigateTo<DebugViewModel>();
            }, o => true);
            NavigateToCollectionCommand = new RelayCommand(o =>
            {
                Navigation.NavigateTo<CollectionViewModel>();
            }, o => true);
            NavigateToDirectoriesManagerCommand = new RelayCommand(o =>
            {
                Navigation.NavigateTo<DirectoriesManagerViewModel>();
            }, o => true);
            NavigateToTagsManagerCommand = new RelayCommand(o =>
            {
                Navigation.NavigateTo<TagsManagerViewModel>();
            }, o => true);
            NavigateToTagCategoriesManagerCommand = new RelayCommand(o =>
            {
                Navigation.NavigateTo<TagCategoriesManagerViewModel>();
            }, o => true);
            NavigateToUnitsManagerCommand = new RelayCommand(o =>
            {
                Navigation.NavigateTo<UnitsManagerViewModel>();
            }, o => true);

            Navigation.NavigateTo<HomeViewModel>();

        }












        //public RelayCommand HomeViewCommand { get; set; }
        //public RelayCommand DebugViewCommand { get; set; }
        //public RelayCommand ChangeBGColorCommand { get; set; }
        //public HomeViewModel HomeVM { get; set; }
        //public DebugViewModel DiscoveryVM { get; set; }

        //private object currentView;

        //public object DragDropControl
        //{
        //    get { return currentView; }
        //    set
        //    {
        //        currentView = value;
        //        OnPropertyChanged();
        //    }
        //}

        //public MainViewModel()
        //{
        //    HomeVM = new HomeViewModel();
        //    DiscoveryVM = new DebugViewModel(this);
        //    Bgcolor = "#999999";


        //    DragDropControl = HomeVM;

        //    ChangeBGColorCommand = new RelayCommand(o =>
        //    {
        //        DragDropControl = HomeVM;
        //    });
        //    HomeViewCommand = new RelayCommand(o =>
        //    {
        //        DragDropControl = HomeVM;
        //    });
        //    DebugViewCommand = new RelayCommand(o =>
        //    {
        //        DragDropControl = DiscoveryVM;
        //    });

        //}
    }
}

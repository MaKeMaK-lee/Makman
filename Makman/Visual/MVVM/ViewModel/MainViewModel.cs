
using Makman.Middle.Core;
using Makman.Visual.Core;

namespace Makman.Visual.MVVM.ViewModel
{
    public class MainViewModel : Core.ViewModel
    {

        private INavigation _navigation; //TODO 300 Cursor statuses


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
        public RelayCommand NavigateToDiscoveryCommand { get; set; }
        public RelayCommand NavigateToCollectionCommand { get; set; }
        public RelayCommand NavigateTodirectoriesManagerCommand { get; set; }

        public MainViewModel(INavigation navigationService)
        {
            Navigation = navigationService;
            NavigateToHomeCommand = new RelayCommand(o =>
            {
                Navigation.NavigateTo<HomeViewModel>();
            }, o => true);
            NavigateToDiscoveryCommand = new RelayCommand(o =>
            {
                Navigation.NavigateTo<DiscoveryViewModel>();
            }, o => true);
            NavigateToCollectionCommand = new RelayCommand(o =>
            {
                Navigation.NavigateTo<CollectionViewModel>();
            }, o => true);
            NavigateTodirectoriesManagerCommand = new RelayCommand(o =>
            {
                Navigation.NavigateTo<DirectoriesManagerViewModel>();
            }, o => true);

            Navigation.NavigateTo<HomeViewModel>();

        }












        //public RelayCommand HomeViewCommand { get; set; }
        //public RelayCommand DiscoveryViewCommand { get; set; }
        //public RelayCommand ChangeBGColorCommand { get; set; }
        //public HomeViewModel HomeVM { get; set; }
        //public DiscoveryViewModel DiscoveryVM { get; set; }

        //private object currentView;

        //public object CurrentView
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
        //    DiscoveryVM = new DiscoveryViewModel(this);
        //    Bgcolor = "#999999";


        //    CurrentView = HomeVM;

        //    ChangeBGColorCommand = new RelayCommand(o =>
        //    {
        //        CurrentView = HomeVM;
        //    });
        //    HomeViewCommand = new RelayCommand(o =>
        //    {
        //        CurrentView = HomeVM;
        //    });
        //    DiscoveryViewCommand = new RelayCommand(o =>
        //    {
        //        CurrentView = DiscoveryVM;
        //    });

        //}
    }
}


using Makman.Middle.Core;
using Makman.Visual.Core;
using System.Windows;

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

        public bool debugMode = false;
        public bool DebugMode
        {
            get => debugMode;
            set
            {
                debugMode = value;
                OnPropertyChanged(nameof(DebugMode));
                OnPropertyChanged(nameof(DebugNavigationVisibility));
            }
        }

        public Visibility DebugNavigationVisibility => debugMode ? Visibility.Visible : Visibility.Collapsed;

        public RelayCommand KeyDownF3Command { get; set; }

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

            KeyDownF3Command = new RelayCommand(o =>
            {
                if (DebugMode)
                    DebugMode = false;
                else
                    DebugMode = true;
            }, o => true);

            Navigation.NavigateTo<HomeViewModel>();
        }
    }
}


using Makman.Middle.Core;
using Makman.Visual.Core;
using Makman.Visual.MVVM.Model;

namespace Makman.Visual.MVVM.ViewModel
{
    public class DebugViewModel : Core.ViewModel
    {
        private INavigation _navigation;
        private readonly IServicesAccessor _servicesAccessor;

        public INavigation Navigation
        {
            get => _navigation;
            set
            {
                _navigation = value;
                OnPropertyChanged();
            }
        }

        public string testString;
        public string TestString
        {
            get => testString;
            set
            {
                testString = value;
                OnPropertyChanged(nameof(testString));
            }
        }

        public RelayCommand ViewInExplorerCommand { get; set; }
        public RelayCommand NavigateToHomeCommand { get; set; }
        public RelayCommand TestFillCommand { get; set; }

        public DebugViewModel(INavigation navigationService, IServicesAccessor servicesAccessor)
        {
            _servicesAccessor = servicesAccessor;

            Navigation = navigationService;
            NavigateToHomeCommand = new RelayCommand(o =>
            {
                Navigation.NavigateTo<HomeViewModel>();
            }, o => true);
            ViewInExplorerCommand = new RelayCommand(o =>
            {

            }, o => true);
            TestFillCommand = new RelayCommand(o =>
            {

            }, o => true);

        }
    }
}

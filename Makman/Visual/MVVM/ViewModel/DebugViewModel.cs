
using Makman.Middle.Core;
using Makman.Middle.Services;
using Makman.Visual.Core;
using Makman.Visual.MVVM.Model;

namespace Makman.Visual.MVVM.ViewModel
{
    public class DebugViewModel : Core.ViewModel
    {
        private INavigation _navigation;
        private readonly IServicesAccessor _servicesAccessor;
        private IFileSystemAccessService _fileSystemAccessService;
        private IUnitMoverService _unitMoverService;

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

        public DebugViewModel(INavigation navigationService, IServicesAccessor servicesAccessor, IFileSystemAccessService fileSystemAccessService, IUnitMoverService unitMoverService)
        {
            _servicesAccessor = servicesAccessor;
            _unitMoverService = unitMoverService;
            _fileSystemAccessService = fileSystemAccessService;

            Navigation = navigationService;
            NavigateToHomeCommand = new RelayCommand(o =>
            {
                Navigation.NavigateTo<HomeViewModel>();
            }, o => true);
            ViewInExplorerCommand = new RelayCommand(o =>
            { 
                //WindowsUse.ViewInExplorer(WindowsUse.GetFilesFromDirectory(WindowsUse.ChooseDirectory()!).Skip(321).First());
            }, o => true);
            TestFillCommand = new RelayCommand(o =>
            {
                _servicesAccessor.DatabaseFill();
            }, o => true);

        }
    }
}

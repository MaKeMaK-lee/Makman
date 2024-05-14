
using Makman.Data.WindowsOS;
using Makman.Middle.Core;
using Makman.Middle.Entities;
using Makman.Middle.Services;
using Makman.Visual.Core;
using Makman.Visual.MVVM.Model;

namespace Makman.Visual.MVVM.ViewModel
{
    public class DiscoveryViewModel : Core.ViewModel
    {
        private INavigation _navigation;
        private readonly IServicesAccessor _servicesAccessor;
        private IFileSystemAccessService _fileSystemAccessService;
        private IFileMoverService _fileMoverService;

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


        public DiscoveryViewModel(INavigation navigationService, IServicesAccessor servicesAccessor, IFileSystemAccessService fileSystemAccessService, IFileMoverService fileMoverService)
        {
            _servicesAccessor = servicesAccessor;
            _fileMoverService = fileMoverService;
            _fileSystemAccessService = fileSystemAccessService;

            Navigation = navigationService;
            NavigateToHomeCommand = new RelayCommand(o =>
            {
                Navigation.NavigateTo<HomeViewModel>();
            }, o => true);
            ViewInExplorerCommand = new RelayCommand(o =>
            {
                _fileMoverService.FilesMoveToDirectory(
                    ["C:\\Users\\Akatsuki\\Desktop\\1\\1.png", "C:\\Users\\Akatsuki\\Desktop\\1\\2.png", "C:\\Users\\Akatsuki\\Desktop\\1\\3.png",],
                         new CollectionDirectory() { Path = "C:\\Users\\Akatsuki\\Desktop", SynchronizingWithCloud = true },
                    newStatusString => { TestString = newStatusString; });
                //WindowsUse.ViewInExplorer(WindowsUse.GetFilesFromDirectory(WindowsUse.ChooseDirectory()!).Skip(321).First());
            }, o => true);
            TestFillCommand = new RelayCommand(o =>
            {
                _servicesAccessor.DatabaseFill();
            }, o => true);



        }
    }
}

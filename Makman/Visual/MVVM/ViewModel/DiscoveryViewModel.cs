﻿
using Makman.Data.WindowsOS;
using Makman.Middle.Core;
using Makman.Visual.Core;
using Makman.Visual.MVVM.Model;

namespace Makman.Visual.MVVM.ViewModel
{
    public class DiscoveryViewModel : Core.ViewModel
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


        public RelayCommand ViewInExplorerCommand { get; set; }
        public RelayCommand NavigateToHomeCommand { get; set; }
        public RelayCommand TestFillCommand { get; set; }


        public DiscoveryViewModel(INavigation navigationService, IServicesAccessor servicesAccessor)
        {
            _servicesAccessor = servicesAccessor;

            Navigation = navigationService;
            NavigateToHomeCommand = new RelayCommand(o =>
            {
                Navigation.NavigateTo<HomeViewModel>();
            }, o => true);
            ViewInExplorerCommand = new RelayCommand(o =>
            {
                WindowsUse.ViewInExplorer(WindowsUse.GetFilesFromDirectory(WindowsUse.ChooseDirectory()!).Skip(321).First());
            }, o => true);
            TestFillCommand = new RelayCommand(o =>
            {
                _servicesAccessor.DatabaseFill();
            }, o => true);



        }
    }
}

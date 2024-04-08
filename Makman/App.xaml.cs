
using Makman.Middle.EntityManagementServices;
using Makman.Middle.Services;
using Makman.Visual.Core;
using Makman.Visual.MVVM.Model;
using Makman.Visual.MVVM.ViewModel;
using Microsoft.Extensions.DependencyInjection;
using System.Windows;

namespace Makman
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private readonly ServiceProvider _serviceProvider;

        public App()
        {
            IServiceCollection services = new ServiceCollection();

            services.AddSingleton<MainWindow>(provider => new MainWindow
            {
                DataContext = provider.GetRequiredService<MainViewModel>(),
                collectionDatabaseService = provider.GetRequiredService<ICollectionDatabaseService>()
            });
            services.AddSingleton<MainViewModel>();
            services.AddSingleton<HomeViewModel>();
            services.AddSingleton<DirectoriesManagerViewModel>();
            services.AddSingleton<CollectionViewModel>();
            services.AddSingleton<DiscoveryViewModel>();
            services.AddSingleton<Func<Type, ViewModel>>
                (serviceProvider => viewModelType => (ViewModel)serviceProvider.GetRequiredService(viewModelType));
            services.AddSingleton<INavigation, Navigation>();
            services.AddSingleton<IServicesAccessor, ServicesAccessor>();

            services.AddSingleton<ICollectionDatabaseService, CollectionDatabaseService>();
            services.AddSingleton<ITestAndDebugService, TestAndDebugService>();
            services.AddSingleton<IWindowsAccessService, WindowsAccessService>();
            services.AddSingleton<IUnitManagementService, UnitManagementService>();
            services.AddSingleton<ICollectionDirectoryManagementService, CollectionDirectoryManagementService>();



            _serviceProvider = services.BuildServiceProvider();




            Startup += Application_Startup;
        }
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            var mainWindow = _serviceProvider.GetRequiredService<MainWindow>();
            mainWindow.Show();

        }
    }

}

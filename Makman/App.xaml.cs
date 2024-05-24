
using Makman.Middle.Core;
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
                _servicesAccessor = provider.GetRequiredService<IServicesAccessor>()
            });
            services.AddSingleton<MainViewModel>();
            services.AddSingleton<HomeViewModel>();
            services.AddSingleton<DirectoriesManagerViewModel>();
            services.AddSingleton<TagsManagerViewModel>();
            services.AddSingleton<TagCategoriesManagerViewModel>();
            services.AddSingleton<CollectionViewModel>();
            services.AddSingleton<DebugViewModel>();
            services.AddSingleton<UnitsManagerViewModel>();
            services.AddSingleton<UnitsAdderViewModel>();
            services.AddSingleton<Func<Type, ViewModel>>
                (serviceProvider => viewModelType => (ViewModel)serviceProvider.GetRequiredService(viewModelType));
            services.AddSingleton<INavigation, Navigation>();

            services.AddSingleton<IServicesAccessor, ServicesAccessor>();

            services.AddSingleton<IConvertService, ConvertService>();
            services.AddSingleton<ISettingsService, SettingsService>();
            services.AddSingleton<ICollectionDatabaseService, CollectionDatabaseService>();
            services.AddSingleton<ITestAndDebugService, TestAndDebugService>();
            services.AddSingleton<IUnitMoverService, UnitMoverService>();
            services.AddSingleton<IFileSystemAccessService, FileSystemAccessService>();

            services.AddSingleton<IUnitManagementService, UnitManagementService>();
            services.AddSingleton<ICollectionDirectoryManagementService, CollectionDirectoryManagementService>();
            services.AddSingleton<ITagManagementService, TagManagementService>();
            services.AddSingleton<ITagCategoryManagementService, TagCategoryManagementService>();
            services.AddSingleton<IBunchManagementService, BunchManagementService>();



            services.AddLazyResolution();

            _serviceProvider = services.BuildServiceProvider();




            Startup += Application_Startup;
        }
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            var mainWindow = _serviceProvider.GetRequiredService<MainWindow>();

            ((IConvertServiceConnectable)Resources["StringColoroBrushConverter"])
                ._ConvertService = _serviceProvider.GetRequiredService<IConvertService>();
            ((IConvertServiceConnectable)Resources["FilePathToImageConverter"])
                ._ConvertService = _serviceProvider.GetRequiredService<IConvertService>();
            ((IConvertServiceConnectable)Resources["FilePathToThumbImageConverter"])
                ._ConvertService = _serviceProvider.GetRequiredService<IConvertService>();

            mainWindow.Show();
        }
    }

}

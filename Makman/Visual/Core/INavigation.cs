
namespace Makman.Visual.Core
{
    public interface INavigation
    {
        ViewModel CurrentView { get; }

        void NavigateTo<T>() where T : ViewModel;
    }
}

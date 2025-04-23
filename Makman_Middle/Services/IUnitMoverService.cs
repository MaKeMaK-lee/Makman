
using Makman_Entities;

namespace Makman_Middle.Services
{
    public interface IUnitMoverService
    {
        void FilesMoveToDirectory(IEnumerable<Unit> filePaths, CollectionDirectory directory, Action<string, bool>? statusUpdateAction = null);
    }
}

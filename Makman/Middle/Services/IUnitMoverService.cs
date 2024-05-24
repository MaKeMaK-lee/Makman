
using Makman.Middle.Entities;

namespace Makman.Middle.Services
{
    public interface IUnitMoverService
    {
        void FilesMoveToDirectory(IEnumerable<Unit> filePaths, CollectionDirectory directory, Action<string, bool>? statusUpdateAction = null);
    }
}

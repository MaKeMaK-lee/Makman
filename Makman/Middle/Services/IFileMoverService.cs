

using Makman.Middle.Entities;

namespace Makman.Middle.Services
{
    public interface IFileMoverService
    {
        void FilesMoveToDirectory(IEnumerable<string> filePaths, CollectionDirectory directory, Action<string>? statusUpdateAction = null);

    }
}

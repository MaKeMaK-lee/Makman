
using Makman.Middle.Entities;
using Makman.Middle.Services;

namespace Makman.Middle.EntityManagementServices
{
    public class CollectionDirectoryManagementService(ICollectionDatabaseService collectionDatabaseService, IFileSystemAccessService fileSystemAccessService) : ICollectionDirectoryManagementService
    {
        readonly ICollectionDatabaseService _collectionDatabaseService = collectionDatabaseService;
        readonly IFileSystemAccessService _fileSystemAccessService = fileSystemAccessService;
        public void AddNew()
        {
            var createdItem = Create();
            if (createdItem == null)
                return;
            _collectionDatabaseService.Add(createdItem);
            _collectionDatabaseService.Save();
        }

        public void Remove(IEnumerable<CollectionDirectory> items)
        {
            foreach (var item in items)
            {
                _collectionDatabaseService.Remove(item);
            }
            _collectionDatabaseService.Save();
        }

        public void PickNewPath(CollectionDirectory collectionDirectory)
        {
            string? path = _fileSystemAccessService.ChooseDirectory();
            if (path == null)
                return;
            if (path == collectionDirectory.Path)
                return;

            collectionDirectory.Path = path;
            collectionDirectory.OnPropertyChanged(nameof(collectionDirectory.Path));
        }

        public CollectionDirectory? Create()
        {
            string? path = _fileSystemAccessService.ChooseDirectory();
            if (path == null)
                return null;

            return new CollectionDirectory(path, synchronizingwithcloud: false);
        }
    }
}

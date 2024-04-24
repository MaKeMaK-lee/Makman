using Makman.Middle.Core;
using Makman.Middle.Entities;
using Makman.Middle.Services;

namespace Makman.Middle.EntityManagementServices
{
    public class CollectionDirectoryManagementService(ICollectionDatabaseService collectionDatabaseService, IFileSystemAccessService windowsAccessService) : ICollectionDirectoryManagementService
    {
        readonly ICollectionDatabaseService _collectionDatabaseService = collectionDatabaseService;
        readonly IFileSystemAccessService _windowsAccessService = windowsAccessService;
        public void AddNew()
        {
            var createdItem = Create();
            if (createdItem == null) { return; }
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
            string? path = _windowsAccessService.ChooseDirectory();
            if (path == null)
                return;
            if (path == collectionDirectory.Path)
                return;

            collectionDirectory.Path = path;
            collectionDirectory.OnPropertyChanged(nameof(collectionDirectory.Path));
        }

        public CollectionDirectory? Create()
        {
            string? path = _windowsAccessService.ChooseDirectory();
            if (path == null) { return null; }

            return new CollectionDirectory(path, synchronizingwithcloud: false);
        }

        //public void SetCommandActionToCommand(CollectionDirectory collectionDirectory)
        //{
        //    collectionDirectory.PickDirectoryCommand = new RelayCommand(collectionDirectory.PickDirectoryCommandAction, o => true);
        //}
    }
}

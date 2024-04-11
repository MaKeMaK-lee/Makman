using Makman.Middle.Core;
using Makman.Middle.Entities;
using Makman.Middle.Services;

namespace Makman.Middle.EntityManagementServices
{
    public class TagManagementService(ICollectionDatabaseService collectionDatabaseService, IWindowsAccessService windowsAccessService) : ITagManagementService
    {
        readonly ICollectionDatabaseService _collectionDatabaseService = collectionDatabaseService;
        readonly IWindowsAccessService _windowsAccessService = windowsAccessService;
        public void AddNew(string name)
        {
            var createdItem = Create(name);
            if (createdItem == null) { return; }
            _collectionDatabaseService.Add(createdItem);
            _collectionDatabaseService.Save();
        }

        public void Remove(IEnumerable<Tag> items)
        {
            foreach (var item in items)
            {
                _collectionDatabaseService.Remove(item);
            }
            _collectionDatabaseService.Save();
        }

        public Tag? Create(string name)
        {
            if (_collectionDatabaseService.IsContainTagWithName(name))
            {
                return null;
            }
            return new Tag(name);
        }
    }
}

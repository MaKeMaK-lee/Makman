using Makman.Middle.Entities;
using Makman.Middle.Services; 

namespace Makman.Middle.EntityManagementServices
{ 
    public class TagCategoryManagementService(ICollectionDatabaseService collectionDatabaseService ) : ITagCategoryManagementService
    {
        readonly ICollectionDatabaseService _collectionDatabaseService = collectionDatabaseService; 
        public void AddNew(string name)
        {
            var createdItem = Create(name);
            if (createdItem == null) { return; }
            _collectionDatabaseService.Add(createdItem);
            _collectionDatabaseService.Save();
        }

        public void Remove(IEnumerable<TagCategory> items)
        {
            foreach (var item in items)
            {
                _collectionDatabaseService.Remove(item);
            }
            _collectionDatabaseService.Save();
        }

        public TagCategory? Create(string name)
        {
            if (name == "" || _collectionDatabaseService.IsContainTagCategoryWithName(name))
            {
                return null;
            }
            return new TagCategory(name);
        }
    }
}

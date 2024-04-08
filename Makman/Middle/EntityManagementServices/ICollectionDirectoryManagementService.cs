using Makman.Middle.Entities;

namespace Makman.Middle.EntityManagementServices
{
    public interface ICollectionDirectoryManagementService
    {
        //void SetCommandActionToCommand(CollectionDirectory collectionDirectory);
        void AddNew();
        CollectionDirectory? Create();
        void PickNewPath(CollectionDirectory collectionDirectory);
        void Remove(IEnumerable<CollectionDirectory> collectionDirectory);
    }
}

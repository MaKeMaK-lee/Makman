using Makman.Middle.Entities;
using Makman.Middle.Services;

namespace Makman.Middle.EntityManagementServices
{
    public class BunchManagementService (ICollectionDatabaseService collectionDatabaseService):IBunchManagementService
    {
        readonly ICollectionDatabaseService _collectionDatabaseService = collectionDatabaseService;
        
        /// <returns>Added new bunch</returns>
        public Bunch AddNew()
        {
            var createdItem = Create();
            _collectionDatabaseService.Add(createdItem);
            _collectionDatabaseService.Save();
            return createdItem;
        }

        public Bunch Create()
        {
            return new Bunch();
        }
    }
}

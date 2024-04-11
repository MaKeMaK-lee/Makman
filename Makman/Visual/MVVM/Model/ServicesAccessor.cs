using Makman.Middle.Entities;
using Makman.Middle.Services;

namespace Makman.Visual.MVVM.Model
{
    public class ServicesAccessor(ITestAndDebugService testAndDebugService, ICollectionDatabaseService collectionDatabaseService) : IServicesAccessor
    {
        private readonly ITestAndDebugService _testAndDebugService = testAndDebugService;
        private readonly ICollectionDatabaseService _collectionDatabaseService = collectionDatabaseService;
        public void DatabaseFill()
        {
            _testAndDebugService.DatabaseFill();
        }

        public IEnumerable<CollectionDirectory> GetDirectories()
        {
            return _collectionDatabaseService.GetCollectionDirectories();
        }

        public IEnumerable<Tag> GetTags()
        {
            return _collectionDatabaseService.GetTags();
        }

        public IEnumerable<TagCategory> GetTagCategories()
        { 
            return _collectionDatabaseService.GetTagCategories();
        }
        
        public IEnumerable<Unit> GetUnits()
        { 
            return _collectionDatabaseService.GetUnits();
        }
    }
}

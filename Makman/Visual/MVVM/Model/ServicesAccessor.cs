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

        public IEnumerable<Unit> GetUnits()
        {

            //var u = _collectionDatabaseService.GetUnits().FirstOrDefault(u=>u.FileName.StartsWith("-EA"));
            //var tags = u.Tags;
            return _collectionDatabaseService.GetUnits();
        }
    }
}

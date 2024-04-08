using Makman.Middle.Entities; 

namespace Makman.Middle.Services
{
    public interface ICollectionDatabaseService : IDisposable
    {
        //public void Load();
        public IEnumerable<Unit> GetUnits(); 
        public IEnumerable<CollectionDirectory> GetCollectionDirectories(); 
        public bool Save();
        public void Add(IEnumerable<Unit> elements);
        public void Add(CollectionDirectory element);
        public void Remove(CollectionDirectory element);









    }
}

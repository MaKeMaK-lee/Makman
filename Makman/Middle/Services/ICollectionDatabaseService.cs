using Makman.Middle.Entities; 

namespace Makman.Middle.Services
{
    public interface ICollectionDatabaseService : IDisposable
    {
        //public void Load();
        public IEnumerable<Unit> GetUnits(); 
        public IEnumerable<CollectionDirectory> GetCollectionDirectories(); 
        public IEnumerable<Tag> GetTags();
        public bool Save();
        public void Add(IEnumerable<Unit> elements);
        public void Add(CollectionDirectory element);
        public void Remove(CollectionDirectory element);
        public void Add(Tag element);
        public void Remove(Tag element);
        public bool IsContainTagWithName(string name);
        
    }
}

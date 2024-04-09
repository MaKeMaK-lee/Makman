using Makman.Data.SQLite;
using Makman.Middle.Entities;

namespace Makman.Middle.Services
{
    public class CollectionDatabaseService : ICollectionDatabaseService
    {
        private bool disposed = false;

        LocalDatabase Database { get; set; }
        public CollectionDatabaseService()
        {

            Database = new();
            Database.Load();
        }

        public void Dispose()
        {
            if (disposed)
            {
                return;
            }

            Database.Dispose();

            GC.SuppressFinalize(this);
            disposed = true;
        }

        //public void Load()
        //{
        //    Database?.Load();
        //}

        public bool Save()
        {
            return Database.Save();
        }

        public void Add(IEnumerable<Unit> elements)
        {
            Database.Add(elements);
        }

        public void Add(CollectionDirectory element)
        {
            Database.Add(element);
        }

        public void Add(Tag element)
        {
            Database.Add(element);
        }

        public void Remove(Tag element)
        {
            Database.Tags.Remove(element);
        }

        public void Remove(CollectionDirectory element)
        {
            Database.CollectionDirectories.Remove(element);
        }

        public IEnumerable<Unit> GetUnits()
        {
            return Database.Units;
        }

        public IEnumerable<CollectionDirectory> GetCollectionDirectories()
        {
            return Database.CollectionDirectories;
        }

        public IEnumerable<Tag> GetTags()
        {
            return Database.Tags;
        }

    }
}

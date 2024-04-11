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

        public bool Save()
        {
            return Database.Save();
        }

        public void Add(IEnumerable<Unit> elements)
        {
            foreach (var item in elements)
            {
                Database.Units.Add(item);
            }
        }

        public void Add(CollectionDirectory element)
        {
            Database.CollectionDirectories.Add(element);
        }

        public void Add(Tag element)
        {
            Database.Tags.Add(element);
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

        public bool IsContainTagWithName(string name)
        {
            return Database.Tags.Any(i => i.Name == name);
        }
    }
}

using Makman.Middle.Entities;

namespace Makman.Middle.Services
{
    public interface ICollectionDatabaseService : IDisposable
    {
        //public void Load();
        public IEnumerable<Unit> GetUnits();
        public IEnumerable<CollectionDirectory> GetCollectionDirectories();
        public CollectionDirectory GetCollectionDirectoryByPath(string path);
        public IEnumerable<Tag> GetTags();
        public IEnumerable<Tag> GetTagsByNamesLower(IEnumerable<string> names);
        public IEnumerable<TagCategory> GetTagCategories();
        public bool Save();
        public void Add(IEnumerable<Unit> elements);
        public void Add(Bunch element);
        public void Add(CollectionDirectory element);
        public void Remove(CollectionDirectory element);
        public void Add(Tag element);
        public void Remove(Tag element);
        public void Add(TagCategory element);
        public void Remove(TagCategory element);
        public bool IsContainTagWithNameLower(string name);
        public bool IsContainTagCategoryWithNameLower(string name);
        public bool IsContainCollectionDirectoryWithPath(string path);

        /// <returns>List of lists of duplicates</returns>
        public IEnumerable<IEnumerable<Unit>> FindUnitsWhereNamesLooksLikeDuplicate();
    }
}

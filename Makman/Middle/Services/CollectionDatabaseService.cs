using Makman.Data.SQLite;
using Makman.Middle.Entities;
using Makman.Visual.Localization;
using System.Text.RegularExpressions;

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

        public void Add(Bunch element)
        {
            Database.Bunchs.Add(element);
        }

        public void Add(Tag element)
        {
            Database.Tags.Add(element);
        }

        public void Add(TagCategory element)
        {
            Database.TagCategories.Add(element);
        }

        public void Remove(TagCategory element)
        {
            Database.TagCategories.Remove(element);
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

        public IEnumerable<TagCategory> GetTagCategories()
        {
            return Database.TagCategories;
        }

        public bool IsContainTagWithName(string name)
        {
            return Database.Tags.Any(i => i.Name.ToLower() == name.ToLower());
        }

        public bool IsContainTagCategoryWithName(string name)
        {
            return Database.TagCategories.Any(i => i.Name.ToLower() == name.ToLower());
        }

        public IEnumerable<IEnumerable<Unit>> FindUnitsWhereNamesLooksLikeDuplicate()
        { 
            string regexPatternString = $"^([\\s\\S]*?)((?= \\(\\d+\\))|(?= — {UIText.u_filesystem_copy}))+([\\s\\S]*?)(\\.[\\s\\S]*?)$";

            IEnumerable<(string name, string ext)> potentiallyMultipliedFileNamesWithExtensions =
                Database.Units
                .Where(u => Regex.Match(u.FileName, regexPatternString).Success)
                .Select(u =>
                {
                    var regexGroups = Regex.Match(u.FileName, regexPatternString).Groups;
                    return (regexGroups[1].Value, regexGroups[4].Value);
                })
                .Distinct();

            return potentiallyMultipliedFileNamesWithExtensions
                .Select(ne =>
                {
                    return Database.Units.Where(u =>
                    {
                        var regexGroups = Regex.Match(u.FileName, regexPatternString).Groups;
                        if ((regexGroups[1].Value == ne.name) && (regexGroups[4].Value == ne.ext))
                            return true;
                        else
                            return false;
                    });
                })
                .Where(list => list.Count() > 1);
        }
          

    }
}


using Makman.Middle.Entities;
using Makman.Middle.Services;

namespace Makman.Middle.EntityManagementServices
{
    public class TagManagementService(ICollectionDatabaseService collectionDatabaseService, IWindowsAccessService windowsAccessService) : ITagManagementService
    {
        readonly ICollectionDatabaseService _collectionDatabaseService = collectionDatabaseService;
        readonly IWindowsAccessService _windowsAccessService = windowsAccessService;
        public void AddNew(string name)
        {
            var createdItem = Create(name);
            if (createdItem == null) { return; }
            _collectionDatabaseService.Add(createdItem);
            _collectionDatabaseService.Save();
        }

        public void Remove(IEnumerable<Tag> items)
        {
            foreach (var item in items)
            {
                _collectionDatabaseService.Remove(item);
            }
            _collectionDatabaseService.Save();
        }

        public Tag? Create(string name)
        {
            if (name == "" || _collectionDatabaseService.IsContainTagWithName(name))
            {
                return null;
            }
            return new Tag(name);
        }

        public bool IsSameOrNullCategory(IEnumerable<Tag>? tags)
        {
            if (tags?.Count() > 0)
            {
#pragma warning disable CS8602 // Разыменование вероятной пустой ссылки.
                var first = tags.FirstOrDefault(t => t.Category != null, null);
#pragma warning restore CS8602 // Разыменование вероятной пустой ссылки.
                if (first == null)
                    return true;
                return tags.Where(t => t.Category != null).All(t => t.Category == first.Category);
            }
            return true;
        }

        public void SetCategory(IEnumerable<Tag> tags, TagCategory? tagCategory)
        {
            foreach (var item in tags)
            {
                item.Category = tagCategory;
            }
        }
    }
}

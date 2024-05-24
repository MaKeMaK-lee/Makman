using Makman.Middle.Entities;

namespace Makman.Middle.EntityManagementServices
{
    public interface ITagManagementService
    { 
        void AddNew(string name);

        Tag? Create(string name);

        /// <returns>True if all tags category is same (some or all may be null) or if tags is null or empty, otherwise false </returns>
        bool IsSameOrNullCategory(IEnumerable<Tag>? tags);

        void Remove(IEnumerable<Tag> tags);

        void SetCategory(IEnumerable<Tag> tags, TagCategory? tagCategory);
    }
}

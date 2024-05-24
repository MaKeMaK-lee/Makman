using Makman.Middle.Entities; 

namespace Makman.Middle.EntityManagementServices
{
    public interface ITagCategoryManagementService
    {
        void AddNew(string name);

        TagCategory? Create(string name);

        void Remove(IEnumerable<TagCategory> tagCategories);
    }
}

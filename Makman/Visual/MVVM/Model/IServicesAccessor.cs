using Makman.Middle.Entities;

namespace Makman.Visual.MVVM.Model
{
    public interface IServicesAccessor
    {
        void DatabaseFill();
        IEnumerable<CollectionDirectory> GetDirectories();
        IEnumerable<Tag> GetTags();
        IEnumerable<Unit> GetUnits();
    }
}

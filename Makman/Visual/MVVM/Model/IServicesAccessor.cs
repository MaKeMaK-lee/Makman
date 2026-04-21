using Makman_Entities;
using Makman_Entities.Settings;

namespace Makman.Visual.MVVM.Model
{
    public interface IServicesAccessor
    {
        void DatabaseFill();

        IEnumerable<CollectionDirectory> GetDirectories();

        IEnumerable<TagCategory> GetTagCategories();

        IEnumerable<Tag> GetTags();

        IEnumerable<Unit> GetUnits();

        IEnumerable<IEnumerable<Unit>> FindUnitsWhereSomeLooksLikeDuplicate();

        IEnumerable<IEnumerable<Unit>> GetUnitsDuplicatedByNames(IEnumerable<Unit> newUnits); 

        void ViewInExplorer(string path);

        Settings GetSettings();

        bool SaveDatabase();

        bool SaveSettings();
    }
}

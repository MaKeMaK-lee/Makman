
using Makman.Middle.Entities; 
using Makman.Visual.MVVM.Model;
using System.Collections; 

namespace Makman.Visual.MVVM.ViewModel
{
    public class CollectionViewModel : Core.ViewModel//TODO стиль и источник текста под картинками, подгрузка тумбов
    {
        private readonly IServicesAccessor _servicesAccessor;

        private IEnumerable<Unit> unitCollection;
        public IEnumerable<Unit> UnitCollection
        {
            get
            {
                return unitCollection;
            }
        }

        public CollectionViewModel(IServicesAccessor servicesAccessor)
        {
            _servicesAccessor = servicesAccessor;



            unitCollection = _servicesAccessor.GetUnits();
        }

        public IList CollectionListView { get; set; }//TODO wtf is it, delete?


    }
}

using Makman.Middle.Core;
using Makman.Middle.Entities;
using Makman.Middle.EntityManagementServices;
using Makman.Visual.Components.ViewModel;
using Makman.Visual.MVVM.Model;

namespace Makman.Visual.MVVM.ViewModel
{
    public class UnitsManagerViewModel : Core.ViewModel
    {
        private readonly IServicesAccessor _servicesAccessor;
        private readonly IUnitManagementService _unitManagementService;

        private IEnumerable<Unit> unitCollection;
        public IEnumerable<Unit> UnitCollection
        {
            get
            {
                return unitCollection;
            }
        }

        private Core.ViewModel? currentSubview;
        public Core.ViewModel? CurrentSubview
        {
            get => currentSubview;
            private set
            {
                currentSubview = value;
                OnPropertyChanged(nameof(CurrentSubview));
            }
        }

        private void ClearCurrentSubview()
        {
            CurrentSubview = null;
        }

        public RelayCommand SearchDuplicatesStartCommand { get; set; }

        private void SetCommands()
        {
            SearchDuplicatesStartCommand = new RelayCommand(o =>
            {
                CurrentSubview = new UnitComparerViewModel()
                {
                    GetOverviewUnitsListsAction = _servicesAccessor.FindUnitsWhereNamesLooksLikeDuplicate,
                    ViewInExplorerAction = filename => _servicesAccessor.ViewInExplorer(filename),
                    LeftFromOutside = false
                };
                (CurrentSubview as UnitComparerViewModel)?.SearchDuplicatesStart((ie1, ie2) => ClearCurrentSubview());
            }, o => true);
        }

        public UnitsManagerViewModel(IServicesAccessor servicesAccessor, IUnitManagementService unitManagementService)
        {
            _servicesAccessor = servicesAccessor;
            _unitManagementService = unitManagementService;

            SetCommands();

            unitCollection = _servicesAccessor.GetUnits();
        }
    }
}

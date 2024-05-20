
using Makman.Middle.Entities;
using Makman.Middle.Entities.Settings;
using Makman.Middle.EntityManagementServices;
using Makman.Visual.Components.ViewModel;
using Makman.Visual.MVVM.Model;

namespace Makman.Visual.MVVM.ViewModel
{
    public class UnitsAdderViewModel : Core.ViewModel
    {
        private readonly IServicesAccessor _servicesAccessor;
        private readonly IUnitManagementService _unitManagementService;

        private Settings Settings { get; set; }
        private CollectionDirectory LatestTargetDirectory { get; set; }

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

        private void SetCurrentSubviewToUnitComparer(IEnumerable<Unit> newUnits, Action<IEnumerable<Unit>, IEnumerable<Unit>> endAction)
        {
            CurrentSubview = new UnitComparerViewModel()
            {
                GetOverviewUnitsListsAction = () =>
                {
                    return _servicesAccessor.GetUnitsDuplicatedByNames(newUnits);
                },
                ViewInExplorerAction = filename => _servicesAccessor.ViewInExplorer(filename),
                LeftFromOutside = true
            };

            (CurrentSubview as UnitComparerViewModel)?.SearchDuplicatesStart(endAction);
        }

        private void SetCurrentSubviewToUnitsAdderMain(Action<string[], CollectionDirectory?> endAction)
        {
            CurrentSubview = new UnitsAdderMainViewModel()
            {
                Settings = Settings
            };
            (CurrentSubview as UnitsAdderMainViewModel)?.StartTryingUnitCreationAction(endAction);
        }

        private void ResetCurrentSubview()
        {
            ClearCurrentSubview();
            Start();
        }

        private void SendNewUnits(IEnumerable<Unit> allCreatedUnits, IEnumerable<Unit> refusedUnits, IEnumerable<Unit> postfixingUnits)
        {
            if (postfixingUnits.Any())
                _unitManagementService.AddNamePostfix(postfixingUnits);

            var newUnits = allCreatedUnits.Where(u => !refusedUnits.Contains(u));

            _unitManagementService.AddUnitsToDatabase(newUnits);

            _unitManagementService.MoveUnitsToDirectory(newUnits, LatestTargetDirectory);

            ResetCurrentSubview();
        }

        private void CheckDuplicates(IEnumerable<Unit> newUnits)
        {
            SetCurrentSubviewToUnitComparer(newUnits, (refusedUnits, postfixingUnits)
                => SendNewUnits(newUnits, refusedUnits, postfixingUnits)
             );
        }

        private void Start()
        {
            SetCurrentSubviewToUnitsAdderMain((filenames, targetDirectory) =>
            {
                if (targetDirectory == null)
                {
                    ResetCurrentSubview();
                    return;
                }
                LatestTargetDirectory = targetDirectory;
                var newUnits = _unitManagementService.CreateMany(filenames);
                CheckDuplicates(newUnits);
            });
        }

        public UnitsAdderViewModel(IServicesAccessor servicesAccessor, IUnitManagementService unitManagementService)
        {
            _servicesAccessor = servicesAccessor;
            _unitManagementService = unitManagementService;

            Settings = _servicesAccessor.GetSettings();



            ResetCurrentSubview();


        }
    }
}


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

        private CollectionDirectory? LatestTargetDirectory { get; set; }

        private Action<string, bool>? WaiterAction { get; set; }


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
            (CurrentSubview as IDisposable)?.Dispose();
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

        private void SetCurrentSubviewToUnitsAdderMain(Action<string[]> endAction)
        {
            CurrentSubview = new UnitsAdderMainViewModel(
                _servicesAccessor.GetTags(),
                _servicesAccessor.GetTagCategories(),
                _servicesAccessor.GetDirectories())
            {
                Settings = Settings
            };
            (CurrentSubview as UnitsAdderMainViewModel)?.StartTryingUnitCreationAction(endAction);
        }

        private void SetCurrentSubviewToWaitingView(Action endAction)
        {
            var waiterViewModel = new WaiterViewModel();
            CurrentSubview = waiterViewModel;

            WaiterAction = (statusString, finished) =>
            {
                waiterViewModel.StatusUpdate(statusString);

                if (finished)
                    endAction();
            };
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

            if (LatestTargetDirectory != null)
            {
                _unitManagementService.StartMovingUnitsToDirectory(newUnits, LatestTargetDirectory, (s, b) => WaiterAction?.Invoke(s, b));
                SetCurrentSubviewToWaitingView(() =>
                {
                    WaiterAction = null;
                    ResetCurrentSubview();
                });
            }
            else
            {
                ResetCurrentSubview();
            }
        }

        private void CheckDuplicates(IEnumerable<Unit> newUnits)
        {
            SetCurrentSubviewToUnitComparer(newUnits, (refusedUnits, postfixingUnits)
                => SendNewUnits(newUnits, refusedUnits, postfixingUnits)
             );
        }

        private void Start()
        {
            SetCurrentSubviewToUnitsAdderMain((filenames) =>
            {
                CollectionDirectory? directory = null;
                if (Settings.TryMoveFilesByDirectoryTagcategoryNameOnAdding && Settings.TagCategoryForBindTagToDirectory != null)
                {
                    var directoryTag = Settings.TagsOnAddingUnits.FirstOrDefault(t => t.Category == Settings.TagCategoryForBindTagToDirectory, null);
                    if (directoryTag != null)
                    {
                        if (Settings.MainDirectory?.Path != null)
                        {
                            directory = new CollectionDirectory(
                                Settings.MainDirectory.Path + "\\" + directoryTag.Name,
                                Settings.MainDirectory.AutoScanning,
                                Settings.MainDirectory.SynchronizingWithCloud);
                        }
                    }
                }
                if (directory == null)
                {
                    if (Settings.TryMoveFilesOnAdding && Settings.CurrentTargetDirectoryToMoveOnAdding != null)
                    {
                        directory = Settings.CurrentTargetDirectoryToMoveOnAdding;
                    }
                }

                LatestTargetDirectory = directory;

                var newUnits = _unitManagementService.CreateMany(
                    filenames,
                    Settings.AddTagsOnAddingUnits ? Settings.TagsOnAddingUnits : [],
                    Settings.ToggleBunchingOnAddingUnits);
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

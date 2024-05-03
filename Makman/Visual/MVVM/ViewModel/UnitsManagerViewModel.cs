using Makman.Middle.Core;
using Makman.Middle.Entities;
using Makman.Middle.EntityManagementServices;
using Makman.Visual.Localization;
using Makman.Visual.MVVM.Model;
using System.Windows;

namespace Makman.Visual.MVVM.ViewModel
{//TODO Create custom view for 
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

        private Visibility searchDuplicatesVisibility;
        public Visibility SearchDuplicatesVisibility
        {
            get => searchDuplicatesVisibility;
            set
            {
                searchDuplicatesVisibility = value;
                OnPropertyChanged(nameof(SearchDuplicatesVisibility));
            }
        }

        public RelayCommand SearchDuplicatesNextToEndCommand { get; set; }
        public RelayCommand SearchDuplicatesPrevToEndCommand { get; set; }
        public RelayCommand SearchDuplicatesNextCommand { get; set; }
        public RelayCommand SearchDuplicatesPrevCommand { get; set; }
        public RelayCommand SearchDuplicatesSkipCommand { get; set; }
        public RelayCommand SearchDuplicatesSkipbackCommand { get; set; }
        public RelayCommand SearchDuplicatesSeekLeftCommand { get; set; }
        public RelayCommand SearchDuplicatesSeekRightCommand { get; set; }
        public RelayCommand SearchDuplicatesStartCommand { get; set; }

        private void SetCommands()
        {
            SearchDuplicatesNextToEndCommand = new RelayCommand(o =>
            {
                OverviewCurrentIndex = OverviewRightUnitsCount;
                RecalculateOverviewUnits();
            }, o => true);
            SearchDuplicatesPrevToEndCommand = new RelayCommand(o =>
            {
                OverviewCurrentIndex = 0;
                RecalculateOverviewUnits();
            }, o => true);
            SearchDuplicatesNextCommand = new RelayCommand(o =>
            {
                OverviewCurrentIndex++;
                RecalculateOverviewUnits();
            }, o => true);
            SearchDuplicatesPrevCommand = new RelayCommand(o =>
            {
                OverviewCurrentIndex--;
                RecalculateOverviewUnits();
            }, o => true);
            SearchDuplicatesSkipCommand = new RelayCommand(o =>
            {
                TemporarilyIgnoreCurrentConflict();

            }, o => true);//TODO group mode SearchDuplicatesSkip Commands
            SearchDuplicatesSkipbackCommand = new RelayCommand(o =>
            {
                TemporarilyIgnoreCurrentConflict(true);

            }, o => true);
            SearchDuplicatesSeekLeftCommand = new RelayCommand(o =>
            {
                if (OverviewLeftUnit != null)
                    _servicesAccessor.ViewInExplorer(OverviewLeftUnit.FullFileName);
            }, o => true);//TODO in-background mode SearchDuplicatesSeek Commands
            SearchDuplicatesSeekRightCommand = new RelayCommand(o =>
            {
                if (OverviewRightUnit != null)
                    _servicesAccessor.ViewInExplorer(OverviewRightUnit.FullFileName);
            }, o => true);
            SearchDuplicatesStartCommand = new RelayCommand(o =>
            {
                RecalculateOverviewUnitsLists();
                SearchDuplicatesMessage = UIText.unit_searchduplicates_message;
                SearchDuplicatesVisibility = Visibility.Visible;

            }, o => true);
        }

        public UnitsManagerViewModel(IServicesAccessor servicesAccessor, IUnitManagementService unitManagementService)
        {
            _servicesAccessor = servicesAccessor;
            _unitManagementService = unitManagementService;

            SetCommands();
            SearchDuplicatesVisibility = Visibility.Collapsed;

            unitCollection = _servicesAccessor.GetUnits();
        }

        #region SearchDuplicates

        private string searchDuplicatesMessage;
        public string SearchDuplicatesMessage
        {
            get => searchDuplicatesMessage;
            set
            {
                searchDuplicatesMessage = value;
                OnPropertyChanged(nameof(SearchDuplicatesMessage));
            }
        }

        public string SearchDuplicatesPageCounter
        {
            get => (OverviewCurrentIndex + 1) + " / " + (OverviewRightUnitsCount);
        }

        private int overviewCurrentIndex;
        /// <summary>
        /// Starts from 0
        /// Not auto update Overview units
        /// </summary>
        public int OverviewCurrentIndex
        {
            get => overviewCurrentIndex;
            set
            {
                overviewCurrentIndex = value;
                OnPropertyChanged(nameof(OverviewCurrentIndex));
                OnPropertyChanged(nameof(SearchDuplicatesPageCounter));
            }
        }

        private void SearchDuplicatesOff()
        {
            SearchDuplicatesVisibility = Visibility.Collapsed;
        }

        private void RecalculateOverviewUnits()
        {
            if (OverviewRightUnitsCount <= 0)
            {
                OverviewLeftUnit = null;
                OverviewRightUnit = null;
                OverviewCurrentIndex = -1;
                SearchDuplicatesOff();
            }
            else
            {
                if (overviewLeftUnits == null || overviewRightUnits == null)
                {
                    OverviewLeftUnit = null;
                    OverviewRightUnit = null;
                    return;
                }
                if (OverviewCurrentIndex >= OverviewRightUnitsCount)
                {
                    OverviewCurrentIndex = OverviewRightUnitsCount - 1;
                }
                if (OverviewCurrentIndex <= 0)
                {
                    OverviewCurrentIndex = 0;
                }
                OverviewLeftUnit = OverviewLeftUnits?[OverviewCurrentIndex];
                OverviewRightUnit = OverviewRightUnits?[OverviewCurrentIndex];
                OnPropertyChanged(nameof(SearchDuplicatesPageCounter));
            }
        }

        private void RecalculateOverviewUnitsLists()
        {
            List<Unit> lefts = new List<Unit>();
            List<Unit> rights = new List<Unit>();

            var lists = _servicesAccessor.FindUnitsWhereNamesLooksLikeDuplicate();
            foreach (var list in lists)
            {
                var firstInGroup = list.First();
                foreach (var unit in list.Skip(1))
                {
                    lefts.Add(firstInGroup);
                    rights.Add(unit);
                }
            }

            OverviewLeftUnits = lefts;
            OverviewRightUnits = rights;

            RecalculateOverviewUnits();
        }

        private void TemporarilyIgnoreCurrentConflict(bool back = false)
        {
            OverviewLeftUnits?.RemoveAt(OverviewCurrentIndex);
            OverviewRightUnits?.RemoveAt(OverviewCurrentIndex);

            if (back)
                OverviewCurrentIndex--;
            RecalculateOverviewUnits();
        }

        private void TemporarilyIgnoreCurrentConflictGroup(bool back = false)
        {
            var left = OverviewLeftUnit;
            var right = OverviewRightUnit;
            for (int i = 0; i < OverviewLeftUnits?.Count; i++)
            {
                if (left == OverviewLeftUnits[i])
                {
                    OverviewLeftUnits?.RemoveAt(i);
                    OverviewRightUnits?.RemoveAt(i);
                    i--;
                }
            }

            if (back)
                OverviewCurrentIndex--;
            RecalculateOverviewUnits();
        }


        public int OverviewRightUnitsCount => OverviewRightUnits?.Count ?? 0;

        private List<Unit>? overviewRightUnits;
        public List<Unit>? OverviewRightUnits
        {
            get => overviewRightUnits;
            set
            {
                overviewRightUnits = value;
                OnPropertyChanged(nameof(OverviewRightUnits));
            }
        }

        public int OverviewLeftUnitsCount => OverviewLeftUnits?.Count ?? 0;

        private List<Unit>? overviewLeftUnits;
        public List<Unit>? OverviewLeftUnits
        {
            get => overviewLeftUnits;
            set
            {
                overviewLeftUnits = value;
                OnPropertyChanged(nameof(OverviewLeftUnits));
            }
        }

        private Unit? overviewRightUnit;
        public Unit? OverviewRightUnit
        {
            get => overviewRightUnit;
            set
            {
                overviewRightUnit = value;
                OnPropertyChanged(nameof(OverviewRightUnit));
            }
        }

        private Unit? overviewLeftUnit;
        public Unit? OverviewLeftUnit
        {
            get => overviewLeftUnit;
            set
            {
                overviewLeftUnit = value;
                OnPropertyChanged(nameof(OverviewLeftUnit));
            }
        }

        #endregion
    }
}

using Makman.Middle.Core;
using Makman.Middle.Entities;
using Makman.Visual.Localization;
using System.Windows;

namespace Makman.Visual.Components.ViewModel
{
    public class UnitComparerViewModel : Core.ViewModel
    {
        public required Func<IEnumerable<IEnumerable<Unit>>> GetOverviewUnitsListsAction { get; set; }
        public required Action<string> ViewInExplorerAction { get; set; }
        public required bool LeftFromOutside { get; set; }
        public Action<IEnumerable<Unit>, IEnumerable<Unit>> SearchDuplicatesEndAction { get; set; }

        public IEnumerable<Unit> RefusedUnits { get; set; }
        public IEnumerable<Unit> PostfixingUnits { get; set; }

        public Visibility RemoveLeftButtonVisibility
        {
            get => LeftFromOutside ? Visibility.Visible : Visibility.Collapsed;
        }

        public Visibility SkipbackButtonVisibility
        {
            get => LeftFromOutside ? Visibility.Collapsed : Visibility.Visible;
        }



        public void SearchDuplicatesStart(Action<IEnumerable<Unit>, IEnumerable<Unit>> searchDuplicatesEndAction)
        {
            SearchDuplicatesEndAction = searchDuplicatesEndAction;

            CalculateOverviewUnitsLists();
            SearchDuplicatesMessage = UIText.unit_searchduplicates_message;

        }

        public RelayCommand SearchDuplicatesRemoveLeftCommand { get; set; }

        public RelayCommand SearchDuplicatesNextToEndCommand { get; set; }
        public RelayCommand SearchDuplicatesPrevToEndCommand { get; set; }
        public RelayCommand SearchDuplicatesNextCommand { get; set; }
        public RelayCommand SearchDuplicatesPrevCommand { get; set; }
        public RelayCommand SearchDuplicatesSkipCommand { get; set; }
        public RelayCommand SearchDuplicatesSkipbackCommand { get; set; }

        public RelayCommand SearchDuplicatesSeekLeftCommand { get; set; }
        public RelayCommand SearchDuplicatesSeekRightCommand { get; set; }

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
                if (LeftFromOutside)
                    IgnoreCurrentConflictGroup();
                else
                    IgnoreCurrentConflict();

            }, o => true);
            SearchDuplicatesSkipbackCommand = new RelayCommand(o =>
            {
                if (LeftFromOutside)
                    IgnoreCurrentConflictGroup(true);
                else
                    IgnoreCurrentConflict(true);

            }, o => true);

            SearchDuplicatesSeekLeftCommand = new RelayCommand(o =>
            {
                if (OverviewLeftUnit != null)
                    ViewInExplorerAction(OverviewLeftUnit.FullFileName);
            }, o => true);
            SearchDuplicatesSeekRightCommand = new RelayCommand(o =>
            {
                if (OverviewRightUnit != null)
                    ViewInExplorerAction(OverviewRightUnit.FullFileName);
            }, o => true);
        }

        public UnitComparerViewModel()
        {
            SetCommands();
            RefusedUnits = [];
            PostfixingUnits = [];
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

        private void RecalculateOverviewUnits()
        {
            if (OverviewRightUnitsCount <= 0)
            {
                OverviewLeftUnit = null;
                OverviewRightUnit = null;
                OverviewCurrentIndex = -1;
                SearchDuplicatesEndAction(RefusedUnits, PostfixingUnits);
            }
            else
            {
                if (OverviewLeftUnits == null || OverviewRightUnits == null)
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

        private void CalculateOverviewUnitsLists()
        {
            List<Unit> lefts = new List<Unit>();
            List<Unit> rights = new List<Unit>();

            var lists = GetOverviewUnitsListsAction();
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

        private void IgnoreCurrentConflict(bool stepBack = false)
        {
            if (LeftFromOutside && OverviewLeftUnits != null)
            {
                if (stepBack)
                    RefusedUnits = RefusedUnits.Append(OverviewLeftUnits[OverviewCurrentIndex]).Distinct();
                else
                    PostfixingUnits = PostfixingUnits.Append(OverviewLeftUnits[OverviewCurrentIndex]).Distinct();
            }

            OverviewLeftUnits?.RemoveAt(OverviewCurrentIndex);
            OverviewRightUnits?.RemoveAt(OverviewCurrentIndex);

            if (stepBack)
                OverviewCurrentIndex--;
            RecalculateOverviewUnits();
        }

        private void IgnoreCurrentConflictGroup(bool stepBack = false)
        {
            var left = OverviewLeftUnit;
            var right = OverviewRightUnit;
            for (int i = 0; i < OverviewLeftUnits?.Count; i++)
            {
                if (left == OverviewLeftUnits[i])
                {
                    if (LeftFromOutside)
                    {
                        if (stepBack)
                            RefusedUnits = RefusedUnits.Append(left).Distinct();
                        else
                            PostfixingUnits = PostfixingUnits.Append(left).Distinct();
                    }


                    OverviewLeftUnits?.RemoveAt(i);
                    OverviewRightUnits?.RemoveAt(i);
                    i--;
                }
            }

            if (stepBack)
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

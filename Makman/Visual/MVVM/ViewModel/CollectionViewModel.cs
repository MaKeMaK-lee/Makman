
using Makman.Middle.Entities;
using Makman.Visual.MVVM.Model;
using Makman.Middle.Core;
using Makman.Middle.EntityManagementServices;
using Makman.Middle.Services;
using System.Windows;

namespace Makman.Visual.MVVM.ViewModel
{
    public class CollectionViewModel : Core.ViewModel
    {
        public int OverviewBunchedUnitsMaxCount { get; } = 16;
        public int OverviewChildUnitsMaxCount { get; } = 16;

        public int CollectionPanelItemHeight { get; } = 150;
        public int CollectionPanelItemWidth { get; } = 95;
        public int CollectionPanelItemImageMaxHeight => CollectionPanelItemHeight - CollectionPanelItemTextMinHeight;
        public int CollectionPanelItemTextMinHeight { get; } = 20;
        public string CollectionPanelItemSize => CollectionPanelItemWidth + ", " + CollectionPanelItemHeight;

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

        private IEnumerable<Tag> tagCollection;
        public IEnumerable<Tag> TagCollection
        {
            get
            {
                return tagCollection;
            }
        }

        public bool tagBinderIsDropDownOpen;
        public bool TagBinderIsDropDownOpen
        {
            get => tagBinderIsDropDownOpen;
            set
            {
                tagBinderIsDropDownOpen = value;
                OnPropertyChanged(nameof(TagBinderIsDropDownOpen));
            }
        }

        public Tag? tagBinderSelectedItem;
        public Tag? TagBinderSelectedItem
        {
            get => tagBinderSelectedItem;
            set
            {
                tagBinderSelectedItem = value;
                OnPropertyChanged(nameof(TagBinderSelectedItem));
            }
        }

        public IEnumerable<Tag> tagBinderTagCollection;
        public IEnumerable<Tag> TagBinderTagCollection
        {
            get => tagBinderTagCollection;
            set
            {
                tagBinderTagCollection = value;
                OnPropertyChanged(nameof(TagBinderTagCollection));
            }
        }

        public IEnumerable<Unit>? SelectedUnitsPrevious { get; set; }

        private IEnumerable<Unit>? selectedUnits;
        public IEnumerable<Unit>? SelectedUnits
        {
            get => selectedUnits;
            set
            {
                selectedUnits = value;
                if (selectedUnits?.Count() == 0)
                {
                    selectedUnits = null;
                }

                if (IsBunchBindingMode)
                {
                    if (_unitManagementService.TryBindToBunch(
                        SelectedUnits,
                        SelectedUnitsPrevious,
                        includeExamples: true))
                        IsBunchBindingMode = false;
                }


                RecalculateSelectedUnitsPrevious();

                OnPropertyChanged(nameof(IsSelectedMoreThanOneItemInCollection));
                OnPropertyChanged(nameof(IsSelectedOnlyOneItemInCollection));
                OnPropertyChanged(nameof(IsSelectedAnyItemInCollection));
                OverviewSourceChangedNotification();

            }
        }



        private void RecalculateSelectedUnitsPrevious()
        {
            if (SelectedUnits?.Count() > 0)
            {
                SelectedUnitsPrevious = SelectedUnits.ToList();
            }
            else
            {
                SelectedUnitsPrevious = null;
            }

        }

        private bool isBunchBindingMode;
        public bool IsBunchBindingMode
        {
            get => isBunchBindingMode;

            set
            {
                isBunchBindingMode = value;
                OnPropertyChanged(nameof(IsBunchBindingMode));
            }
        }

        public bool IsSelectedMoreThanOneItemInCollection
        {
            get
            {
                if (SelectedUnits == null)
                    return false;
                if (SelectedUnits.Count() > 1)
                    return true;
                return false;
            }
        }

        public bool IsSelectedOnlyOneItemInCollection
        {
            get
            {
                if (SelectedUnits == null)
                    return false;
                if (SelectedUnits.Count() == 1)
                    return true;
                return false;

            }
        }

        public Visibility IsSelectedAnyItemInCollectionToVisibleOrHidden
        {
            get
            {
                if (IsSelectedAnyItemInCollection)
                    return Visibility.Visible;
                return Visibility.Hidden;
            }
        }

        public bool IsSelectedAnyItemInCollection
        {
            get
            {
                if (SelectedUnits == null)
                    return false;
                else
                    return true;
            }
        }

        public RelayCommand TagBindCommand { get; set; }
        public RelayCommand TagUnbindCommand { get; set; }
        public RelayCommand ListViewSelectionChangedCommand { get; set; }

        private void SetCommands()
        {
            ListViewSelectionChangedCommand = new RelayCommand(o =>
            {
                if (o == null)
                    SelectedUnits = null;
                else
                {
                    SelectedUnits = ((IEnumerable<object>)o).Cast<Unit>();
                }
            }, o => true);

            TagBindCommand = new RelayCommand(o =>
            {
                _unitManagementService.TryBindTag(SelectedUnits, TagBinderSelectedItem);
                OnPropertyChanged(nameof(OverviewTags));
            }, o => true);

            TagUnbindCommand = new RelayCommand(o =>
            {
                _unitManagementService.TryUnbindTag(SelectedUnits, TagBinderSelectedItem);
                OnPropertyChanged(nameof(OverviewTags));
            }, o => true);
        }

        public CollectionViewModel(IServicesAccessor servicesAccessor, IUnitManagementService unitManagementService, IFileSystemAccessService fileSystemAccessService)
        {
            _servicesAccessor = servicesAccessor;
            _unitManagementService = unitManagementService;

            SetCommands();

            unitCollection = _servicesAccessor.GetUnits();
            tagCollection = _servicesAccessor.GetTags();

            TagBinderTagCollection = tagCollection.OrderBy(t => t.Name);
        }


        #region overview

        public void OverviewSourceChangedNotification()
        {
            OnPropertyChanged(nameof(OverviewId));
            OnPropertyChanged(nameof(OverviewFileSize));
            OnPropertyChanged(nameof(OverviewPicturePath));
            OnPropertyChanged(nameof(OverviewFullFileName));
            OnPropertyChanged(nameof(OverviewFileName));
            OnPropertyChanged(nameof(OverviewTags));
            OnPropertyChanged(nameof(OverviewBunchedUnits));
            OnPropertyChanged(nameof(OverviewParentUnits));
            OnPropertyChanged(nameof(OverviewChildUnits));
            OnPropertyChanged(nameof(OverviewCount));
        }
        public string OverviewId
        {
            get
            {
                if (SelectedUnits?.Count() > 0)
                {
                    if (SelectedUnits.Count() < 2)
                    {
                        return SelectedUnits.First().Id.ToString();
                    }
                    return Localization.UIText.co_unitoverview_multiselected_id;
                }
                return Localization.UIText.co_unitoverview_nonselected_id;
            }
        }

        public string OverviewPicturePath
        {
            get
            {
                if (SelectedUnits?.Count() > 0)
                {
                    if (SelectedUnits.Count() < 2)
                    {
                        return SelectedUnits.First().FullFileName;
                    }
                    return "";
                }
                return "";
            }
        }

        public string OverviewFullFileName
        {
            get
            {
                if (SelectedUnits?.Count() > 0)
                {
                    if (SelectedUnits.Count() < 2)
                    {
                        return SelectedUnits.First().FullFileName;
                    }
                    return Localization.UIText.co_unitoverview_multiselected_fullfilename;
                }
                return Localization.UIText.co_unitoverview_nonselected_fullfilename;
            }
        }

        public string OverviewFileName
        {
            get
            {
                if (SelectedUnits?.Count() > 0)
                {
                    if (SelectedUnits.Count() < 2)
                    {
                        return SelectedUnits.First().FileName;
                    }
                    return Localization.UIText.co_unitoverview_multiselected_filename;
                }
                return Localization.UIText.co_unitoverview_nonselected_filename;
            }
        }

        public string OverviewFileSize
        {
            get
            {
                if (SelectedUnits?.Count() > 0)
                {
                    if (SelectedUnits.Count() < 2)
                    {
                        return SelectedUnits.First().FileSize.ToString();
                    }
                    long size = 0;
                    foreach (var item in SelectedUnits)
                    {
                        size += item.FileSize;
                    }
                    return size.ToString();
                }
                return Localization.UIText.co_unitoverview_nonselected_filesize;
            }
        }

        public IEnumerable<Tag> OverviewTags
        {
            get
            {
                if (SelectedUnits?.Count() > 0)
                {
                    if (SelectedUnits.Count() < 2)
                    {
                        return SelectedUnits.First().Tags;
                    }
                    var tags = new List<Tag>();
                    foreach (var item in SelectedUnits)
                    {
                        tags.AddRange(item.Tags);
                    }
                    return tags.Distinct();
                }
                return [];
            }
        }

        public IEnumerable<Unit> OverviewBunchedUnits
        {
            get
            {
                if (SelectedUnits?.Count() > 0)
                {
                    if (SelectedUnits.Count() < 2)
                    {
                        var b = SelectedUnits.First().Bunch;
                        if (b != null)
                            return b.Units;
                        else
                            return [];
                    }
                    var units = new List<Unit>();
                    foreach (var item in SelectedUnits.DistinctBy(item => item.Bunch))
                    {
                        if (item.Bunch != null)
                            units.AddRange(item.Bunch.Units);
                        if (units.Count > OverviewBunchedUnitsMaxCount)
                            break;
                    }
                    return units;
                }
                return [];
            }
        }

        public IEnumerable<Unit> OverviewParentUnits
        {
            get
            {
                if (SelectedUnits?.Count() > 0)
                {
                    if (SelectedUnits.Count() < 2)
                    {
                        var u = SelectedUnits.First().ParentUnit;
                        if (u != null)
                            return new List<Unit>() { u };
                        else
                            return [];
                    }
                    var units = new List<Unit>();
                    foreach (var item in SelectedUnits.DistinctBy(item => item.ParentUnit))
                    {
                        if (item.ParentUnit != null)
                            units.Add(item.ParentUnit);
                    }
                    return units;
                }
                return [];
            }
        }

        public int OverviewCount
        {
            get
            {
                if (SelectedUnits?.Count() > 0)
                {
                    return SelectedUnits.Count();
                }
                return 0;
            }
        }

        public IEnumerable<Unit> OverviewChildUnits
        {
            get
            {
                if (SelectedUnits?.Count() > 0)
                {
                    if (SelectedUnits.Count() < 2)
                    {
                        return SelectedUnits.First().ChildUnits;
                    }
                    var units = new List<Unit>();
                    foreach (var item in SelectedUnits)
                    {
                        if (item.ChildUnits != null)
                            units.AddRange(item.ChildUnits);
                        if (units.Count > OverviewChildUnitsMaxCount)
                            break;
                    }
                    return units;
                }
                return [];
            }
        }
        #endregion
    }
}


using Makman.Middle.Entities;
using Makman.Visual.MVVM.Model;
using Makman.Middle.Core;
using Makman.Middle.EntityManagementServices;

namespace Makman.Visual.MVVM.ViewModel
{
    public class CollectionViewModel : Core.ViewModel//TODO стиль и источник текста под картинками, подгрузка тумбов
    {
        const int OverviewItemHeight = 60;

        const int OverviewBunchedUnitsMaxCount = 16;
        const int OverviewChildUnitsMaxCount = 16;

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
                    //TODO 200: ui notificating user about result
                    if (_unitManagementService.TryBindToBunch(
                        SelectedUnits,
                        SelectedUnitsPrevious,
                        includeExamples: true))
                        IsBunchBindingMode = false;
                }


                RecalculateSelectedUnitsPrevious();

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

        //TODO 150 Update this: save bunch when set it to true and apply this bunch until set it to false, or something else
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

        public RelayCommand ListViewSelectionChangedCommand { get; set; }

        public CollectionViewModel(IServicesAccessor servicesAccessor, IUnitManagementService unitManagementService)
        {
            _servicesAccessor = servicesAccessor;
            _unitManagementService = unitManagementService;


            ListViewSelectionChangedCommand = new RelayCommand(o =>
            {
                if (o == null)
                    SelectedUnits = null;
                else
                {
                    SelectedUnits = ((IEnumerable<object>)o).Cast<Unit>();
                }
            }, o => true);

            unitCollection = _servicesAccessor.GetUnits();
        }


        #region overview

        public void OverviewSourceChangedNotification()
        {
            OnPropertyChanged(nameof(OverviewId));
            OnPropertyChanged(nameof(OverviewPicturePath));
            OnPropertyChanged(nameof(OverviewFullFileName));
            OnPropertyChanged(nameof(OverviewFileName));
            OnPropertyChanged(nameof(OverviewBunchedUnits));
            OnPropertyChanged(nameof(OverviewParentUnits));
            OnPropertyChanged(nameof(OverviewChildUnits));
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
                    return tags;
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

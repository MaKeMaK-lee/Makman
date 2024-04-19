
using Makman.Middle.Entities;
using Makman.Visual.MVVM.Model;
using System.Collections;
using Makman.Middle.Core;

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


        public Unit? SelectedUnitsBack { get; set; }  
        public Unit? SelectedUnit { get; set; }  

        public IEnumerable<Unit>? selectedUnits;
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

                if (selectedUnits?.Count() < 0)
                {
                    SelectedUnitsBack = selectedUnits.First();
                }

                OnPropertyChanged(nameof(IsSelectedOnlyOneItemInCollection));
                OnPropertyChanged(nameof(IsSelectedAnyItemInCollection));
                OverviewSourceChangedNotification();

            }
        }

        public bool check;
        public bool Check
        {
            get => check;
            
            set
            {
                check = value;
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

        public CollectionViewModel(IServicesAccessor servicesAccessor)
        {
            _servicesAccessor = servicesAccessor;


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
                    foreach (var item in SelectedUnits)
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
                    }
                    return units;
                }
                return [];
            }
        }
        #endregion
    }
}

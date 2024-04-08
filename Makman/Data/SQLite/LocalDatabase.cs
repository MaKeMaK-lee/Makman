
using Makman.Middle.Entities;
using Makman.Middle.Entities.DatabaseContexts;
using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;

namespace Makman.Data.SQLite
{
    internal class LocalDatabase : IDisposable//TODO 450 Наверн всё же поделить на unitmanager и тп
    {
        private bool disposed = false;
        private LocalDatabaseContext _dbContext;

        enum DataType
        {
            Unit,
            Tag,
            Bunch,
            TagCategory,
            CollectionDirectory
        }
        //TODO 5 пофиксить  инкапс приват сделать
        internal ObservableCollection<Bunch> Bunchs
        {
            private set;
            get;
        }
        internal ObservableCollection<TagCategory> TagCategories
        {
            private set;
            get;
        }
        internal ObservableCollection<CollectionDirectory> CollectionDirectories
        {
            private set;
            get;
        }
        internal ObservableCollection<Tag> Tags
        {
            private set;
            get;
        }
        internal ObservableCollection<Unit> Units
        {
            private set;
            get;
        }

        internal LocalDatabase()
        {
            Init();
        }
        private void Init()
        {
            _dbContext = new LocalDatabaseContext("Collection.db");
            _dbContext.Database.EnsureCreated();
            LoadData();
        }

        private void LoadData()
        {
            _dbContext.Units.AsSplitQuery().Load();
            _dbContext.Tags.AsSplitQuery().Load();
            _dbContext.TagCategories.AsSplitQuery().Load();
            _dbContext.CollectionDirectories.AsSplitQuery().Load();
            _dbContext.Bunchs.AsSplitQuery().Load();


            Units = _dbContext.Units.Local.ToObservableCollection();
            Tags = _dbContext.Tags.Local.ToObservableCollection();
            TagCategories = _dbContext.TagCategories.Local.ToObservableCollection();
            CollectionDirectories = _dbContext.CollectionDirectories.Local.ToObservableCollection();
            Bunchs = _dbContext.Bunchs.Local.ToObservableCollection();
        }
        /*
        private void LoadActualDataOld(DataType? dataType = null)
        {
            if (dataType == null || dataType == DataType.Unit)
            {
                Units = _dbContext.Units.Local.ToObservableCollection();
            }
            if (dataType == null || dataType == DataType.Tag)
            {
                //Tags = _dbContext.Tags.AsTracking();
            }
            if (dataType == null || dataType == DataType.TagCategory)
            {
                //TagCategories = _dbContext.TagCategories.AsTracking();
            }
            if (dataType == null || dataType == DataType.Bunch)
            {
                //Bunchs = _dbContext.Bunchs.AsTracking();
            }
            if (dataType == null || dataType == DataType.CollectionDirectory)
            {
                CollectionDirectories = _dbContext.CollectionDirectories.Local.ToObservableCollection();
            }
        }*/
        private void AddRecords(IEnumerable<Unit> recordsToAdd)
        {
            _dbContext.Units.AddRange(recordsToAdd);
        }
        private void AddRecords(IEnumerable<Tag> recordsToAdd)
        {
            _dbContext.Tags.AddRange(recordsToAdd);
        }
        private void AddRecords(IEnumerable<Bunch> recordsToAdd)
        {
            _dbContext.Bunchs.AddRange(recordsToAdd);
        }
        private void AddRecords(IEnumerable<TagCategory> recordsToAdd)
        {
            _dbContext.TagCategories.AddRange(recordsToAdd);
        }
        private void AddRecords(IEnumerable<CollectionDirectory> recordsToAdd)
        {
            _dbContext.CollectionDirectories.AddRange(recordsToAdd);
        }
        private bool SaveChanges()
        {
            try
            {
                _dbContext.SaveChanges();

            }
            catch (Exception)
            {

                return false;
            }
            return true;
        }





        internal void Add(IEnumerable<Unit> recordsToAdd)
        {
            foreach (var item in recordsToAdd)
            {
                Units.Add(item);

            }
            //AddRecords(recordsToAdd);

            //SaveChanges();
            //LoadActualData(DataType.Unit);
        }
        internal void Add(IEnumerable<Tag> recordsToAdd)
        {
            //AddRecords(recordsToAdd);

            //SaveChanges();
            //LoadActualData(DataType.Tag);
        }
        internal void Add(IEnumerable<TagCategory> recordsToAdd)
        {
            //AddRecords(recordsToAdd);

            //SaveChanges();
            //LoadActualData(DataType.TagCategory);
        }
        internal void Add(IEnumerable<Bunch> recordsToAdd)
        {

            //AddRecords(recordsToAdd);

            //SaveChanges();
            //LoadActualData(DataType.Bunch);
        }
        internal void Add(CollectionDirectory recordToAdd)
        {
            CollectionDirectories.Add(recordToAdd);

            //AddRecords(recordsToAdd);

            //SaveChanges();
            //LoadActualData(DataType.CollectionDirectory);
        }

        internal void Load()
        {
            LoadData();
        }

        /// <summary>
        /// Save current database state to file
        /// </summary>
        /// <returns>True if saved</returns>
        internal bool Save()//bool reloadData = true //TODO 500 почистить 
        {
            return SaveChanges();
            //if (reloadData)
            //{
            //    LoadData();
            //}
        }



        public void Dispose()
        {
            if (disposed)
            {
                return;
            }


            _dbContext.Dispose();

            GC.SuppressFinalize(this);
            disposed = true;

        }
    }
}

//string fileName = "UnitsData.json";
//if (WindowsUse.FileExists(fileName))
//{
//    string jsonString = WindowsUse.ReadAllText(fileName);
//    Units = JsonSerializer.Deserialize<List<Unit>>(jsonString)!;
//}   
//string fileName = "UnitsData.json";
//string jsonString = JsonSerializer.Serialize(Units);
//WindowsUse.WriteAllText(fileName, jsonString);


//private void Add(object recordsToAdd)
//{
//    if (recordsToAdd as IEnumerable<Unit> != null)
//    {
//        _dbContext.Units.AddRange((recordsToAdd as IEnumerable<Unit>)!);
//    }
//    if (recordsToAdd as IEnumerable<Tag> != null)
//    {
//        _dbContext.Tags.AddRange((recordsToAdd as IEnumerable<Tag>)!);
//    }
//    if (recordsToAdd as IEnumerable<Bunch> != null)
//    {
//        _dbContext.Bunchs.AddRange((recordsToAdd as IEnumerable<Bunch>)!);
//    }
//    if (recordsToAdd as IEnumerable<TagCategory> != null)
//    {
//        _dbContext.TagCategories.AddRange((recordsToAdd as IEnumerable<TagCategory>)!);
//    }
//    if (recordsToAdd as IEnumerable<CollectionDirectory> != null)
//    {
//        _dbContext.CollectionDirectories.AddRange((recordsToAdd as IEnumerable<CollectionDirectory>)!);
//    }
//}


//switch (dataType)
//{
//    case DataType.Unit:
//        Units = _dbContext.Units.AsTracking();
//        break;
//    case DataType.Tag:
//        Tags = _dbContext.Tags.AsTracking();
//        break;
//    case DataType.TagCategory:
//        TagCategories = _dbContext.TagCategories.AsTracking();
//        break;
//    case DataType.Bunch:
//        Bunchs = _dbContext.Bunchs.AsTracking();
//        break;
//    case DataType.CollectionDirectory:
//        CollectionDirectories = _dbContext.CollectionDirectories.AsTracking();
//        break;
//    default:
//        Units = _dbContext.Units.AsTracking();
//        Tags = _dbContext.Tags.AsTracking();
//        TagCategories = _dbContext.TagCategories.AsTracking();
//        Bunchs = _dbContext.Bunchs.AsTracking();
//        CollectionDirectories = _dbContext.CollectionDirectories.AsTracking();
//        break;
//}

using Makman.Middle.Entities;
using Makman.Middle.Entities.DatabaseContexts;
using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;

namespace Makman.Data.SQLite
{
    internal class LocalDatabase : IDisposable
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
         
        internal void Load()
        {
            LoadData();
        }

        /// <summary>
        /// Save current database state to file
        /// </summary>
        /// <returns>True if saved</returns>
        internal bool Save()  
        {
            return SaveChanges(); 
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

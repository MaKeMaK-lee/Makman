using Makman.Middle.Entities;
using Makman.Middle.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Makman.Middle.EntityManagementServices
{
    public class BunchManagementService (ICollectionDatabaseService collectionDatabaseService):IBunchManagementService
    {
        readonly ICollectionDatabaseService _collectionDatabaseService = collectionDatabaseService;
        
        /// <returns>Added new bunch</returns>
        public Bunch AddNew()
        {
            var createdItem = Create();
            _collectionDatabaseService.Add(createdItem);
            _collectionDatabaseService.Save();
            return createdItem;
        }

        //public void Remove(IEnumerable<Bunch> items)
        //{
        //    foreach (var item in items)
        //    {
        //        _collectionDatabaseService.Remove(item);
        //    }
        //    _collectionDatabaseService.Save();
        //}

        public Bunch Create()
        {
            return new Bunch();
        }
    }
}

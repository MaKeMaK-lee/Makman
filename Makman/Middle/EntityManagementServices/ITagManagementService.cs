using Makman.Middle.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Makman.Middle.EntityManagementServices
{
    public interface ITagManagementService
    { 
        void AddNew(string name);
        Tag? Create(string name); 
        void Remove(IEnumerable<Tag> collectionDirectory);
    }
}

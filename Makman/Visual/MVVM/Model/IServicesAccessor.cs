using Makman.Middle.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Makman.Visual.MVVM.Model
{
    public interface IServicesAccessor
    {
        void DatabaseFill();
        IEnumerable<CollectionDirectory> GetDirictories();
        IEnumerable<Unit> GetUnits();
    }
}

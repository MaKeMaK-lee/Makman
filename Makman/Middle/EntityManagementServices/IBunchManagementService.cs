using Makman.Middle.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Makman.Middle.EntityManagementServices
{
    public interface IBunchManagementService
    {
        Bunch AddNew();

        Bunch Create();

        //void Remove(IEnumerable<Bunch> bunchs);
    }
}

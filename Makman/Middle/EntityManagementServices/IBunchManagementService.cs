using Makman.Middle.Entities;

namespace Makman.Middle.EntityManagementServices
{
    public interface IBunchManagementService
    {
        Bunch AddNew();

        Bunch Create();
    }
}

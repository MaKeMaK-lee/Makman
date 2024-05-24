
using Makman.Middle.Entities.Settings;

namespace Makman.Middle.Services
{
    public interface ISettingsService
    {
        bool Save();

        Settings CurrentSettings { get; set; }
    }
}

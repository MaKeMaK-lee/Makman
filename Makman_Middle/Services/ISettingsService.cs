
using Makman_Entities.Settings;

namespace Makman_Middle.Services
{
    public interface ISettingsService
    {
        bool Save();

        Settings CurrentSettings { get; set; }
    }
}

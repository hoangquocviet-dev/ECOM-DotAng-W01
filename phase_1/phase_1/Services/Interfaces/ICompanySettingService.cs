using phase_1.Models;
using System.Threading.Tasks;

namespace phase_1.Services.Interfaces
{
    public interface ICompanySettingService
    {
        Task<CompanySetting> GetSettingsAsync();
        Task<CompanySetting> UpdateSettingsAsync(CompanySetting settings);
    }
}

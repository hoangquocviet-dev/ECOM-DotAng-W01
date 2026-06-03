using phase_1.Models;
using System.Threading.Tasks;

namespace phase_1.Repositories
{
    public interface ICompanySettingRepository
    {
        Task<CompanySetting?> GetSettingsAsync();
        Task UpdateSettingsAsync(CompanySetting settings);
        Task AddSettingsAsync(CompanySetting settings);
    }
}

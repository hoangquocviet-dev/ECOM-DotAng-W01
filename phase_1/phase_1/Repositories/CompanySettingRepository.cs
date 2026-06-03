using Microsoft.EntityFrameworkCore;
using phase_1.Data;
using phase_1.Models;
using System.Threading.Tasks;

namespace phase_1.Repositories
{
    public class CompanySettingRepository : ICompanySettingRepository
    {
        private readonly ApplicationDbContext _context;
        public CompanySettingRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<CompanySetting?> GetSettingsAsync()
        {
            return await _context.CompanySettings.FirstOrDefaultAsync();
        }

        public async Task UpdateSettingsAsync(CompanySetting settings)
        {
            _context.CompanySettings.Update(settings);
            await _context.SaveChangesAsync();
        }

        public async Task AddSettingsAsync(CompanySetting settings)
        {
            await _context.CompanySettings.AddAsync(settings);
            await _context.SaveChangesAsync();
        }
    }
}

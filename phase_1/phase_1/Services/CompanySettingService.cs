using phase_1.Models;
using phase_1.Repositories;
using phase_1.Services.Interfaces;
using System.Threading.Tasks;

namespace phase_1.Services
{
    public class CompanySettingService : ICompanySettingService
    {
        private readonly ICompanySettingRepository _repository;
        public CompanySettingService(ICompanySettingRepository repository)
        {
            _repository = repository;
        }

        public async Task<CompanySetting> GetSettingsAsync()
        {
            var settings = await _repository.GetSettingsAsync();
            if (settings == null)
            {
                // Tự động tạo bản ghi mặc định nếu chưa có
                settings = new CompanySetting
                {
                    CompanyName = "Văn Phòng Phẩm ABC",
                    Address = "123 Đường Tôn Đức Thắng, Hà Nội",
                    Hotline = "0909.123.456",
                    Email = "lienhe@vpp-abc.com",
                    WorkingHours = "08:00 - 18:00 từ Thứ 2 đến Thứ 7"
                };
                await _repository.AddSettingsAsync(settings);
            }
            return settings;
        }

        public async Task<CompanySetting> UpdateSettingsAsync(CompanySetting settings)
        {
            var current = await _repository.GetSettingsAsync();
            if (current == null)
            {
                await _repository.AddSettingsAsync(settings);
                return settings;
            }

            // Update fields
            current.CompanyName = settings.CompanyName;
            current.Address = settings.Address;
            current.Hotline = settings.Hotline;
            current.Email = settings.Email;
            current.FacebookLink = settings.FacebookLink;
            current.ZaloLink = settings.ZaloLink;
            current.WorkingHours = settings.WorkingHours;

            await _repository.UpdateSettingsAsync(current);
            return current;
        }
    }
}

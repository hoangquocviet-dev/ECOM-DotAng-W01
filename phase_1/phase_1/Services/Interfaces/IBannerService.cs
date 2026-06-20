using phase_1.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace phase_1.Services.Interfaces
{
    public interface IBannerService
    {
        Task<IEnumerable<BannerDto>> GetAllAsync();
        Task<IEnumerable<BannerDto>> GetActiveBannersAsync();
        Task<BannerDto?> GetByIdAsync(int id);
        Task<BannerDto> CreateAsync(CreateBannerRequest request);
        Task<BannerDto?> UpdateAsync(int id, UpdateBannerRequest request);
        Task<bool> DeleteAsync(int id);
    }
}

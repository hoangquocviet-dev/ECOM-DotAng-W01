using phase_1.DTOs;
using phase_1.Models;
using phase_1.Repositories;
using phase_1.Services.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace phase_1.Services
{
    public class BannerService : IBannerService
    {
        private readonly IBannerRepository _bannerRepository;

        public BannerService(IBannerRepository bannerRepository)
        {
            _bannerRepository = bannerRepository;
        }

        public async Task<IEnumerable<BannerDto>> GetAllAsync()
        {
            var banners = await _bannerRepository.GetAllAsync();
            return banners.Select(b => MapToDto(b));
        }

        public async Task<IEnumerable<BannerDto>> GetActiveBannersAsync()
        {
            var banners = await _bannerRepository.GetActiveBannersAsync();
            return banners.Select(b => MapToDto(b));
        }

        public async Task<BannerDto?> GetByIdAsync(int id)
        {
            var banner = await _bannerRepository.GetByIdAsync(id);
            if (banner == null) return null;
            return MapToDto(banner);
        }

        public async Task<BannerDto> CreateAsync(CreateBannerRequest request)
        {
            var banner = new Banner
            {
                Title = request.Title,
                ImageUrl = request.ImageUrl,
                TargetUrl = request.TargetUrl,
                IsActive = request.IsActive,
                OrderIndex = request.OrderIndex
            };

            var createdBanner = await _bannerRepository.CreateAsync(banner);
            return MapToDto(createdBanner);
        }

        public async Task<BannerDto?> UpdateAsync(int id, UpdateBannerRequest request)
        {
            var banner = await _bannerRepository.GetByIdAsync(id);
            if (banner == null) return null;

            banner.Title = request.Title;
            banner.ImageUrl = request.ImageUrl;
            banner.TargetUrl = request.TargetUrl;
            banner.IsActive = request.IsActive;
            banner.OrderIndex = request.OrderIndex;

            await _bannerRepository.UpdateAsync(banner);
            return MapToDto(banner);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var banner = await _bannerRepository.GetByIdAsync(id);
            if (banner == null) return false;

            await _bannerRepository.DeleteAsync(banner);
            return true;
        }

        private BannerDto MapToDto(Banner banner)
        {
            return new BannerDto
            {
                Id = banner.Id,
                Title = banner.Title,
                ImageUrl = banner.ImageUrl,
                TargetUrl = banner.TargetUrl,
                IsActive = banner.IsActive,
                OrderIndex = banner.OrderIndex,
                CreatedAt = banner.CreatedAt
            };
        }
    }
}

using phase_1.DTOs;
using phase_1.Models;
using phase_1.Repositories;
using phase_1.Services.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace phase_1.Services
{
    public class BrandService : IBrandService
    {
        private readonly IBrandRepository _brandRepository;

        public BrandService(IBrandRepository brandRepository)
        {
            _brandRepository = brandRepository;
        }

        public async Task<IEnumerable<Brand>> GetAllBrandsAsync()
        {
            return await _brandRepository.GetAllBrandsAsync();
        }

        public async Task<Brand?> GetBrandByIdAsync(int id)
        {
            return await _brandRepository.GetBrandByIdAsync(id);
        }

        public async Task<Brand> CreateBrandAsync(CreateBrandRequest request)
        {
            var brand = new Brand
            {
                Name = request.Name,
                Description = request.Description
            };

            await _brandRepository.AddBrandAsync(brand);
            return brand;
        }

        public async Task<Brand?> UpdateBrandAsync(int id, UpdateBrandRequest request)
        {
            var brand = await _brandRepository.GetBrandByIdAsync(id);
            if (brand == null) return null;

            brand.Name = request.Name;
            brand.Description = request.Description;

            await _brandRepository.UpdateBrandAsync(brand);
            return brand;
        }

        public async Task<bool> DeleteBrandAsync(int id)
        {
            var brand = await _brandRepository.GetBrandByIdAsync(id);
            if (brand == null) return false;

            await _brandRepository.DeleteBrandAsync(brand);
            return true;
        }
    }
}

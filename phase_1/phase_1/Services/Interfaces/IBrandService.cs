using phase_1.DTOs;
using phase_1.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace phase_1.Services.Interfaces
{
    public interface IBrandService
    {
        Task<IEnumerable<Brand>> GetAllBrandsAsync();
        Task<Brand?> GetBrandByIdAsync(int id);
        Task<Brand> CreateBrandAsync(CreateBrandRequest request);
        Task<Brand?> UpdateBrandAsync(int id, UpdateBrandRequest request);
        Task<bool> DeleteBrandAsync(int id);
    }
}

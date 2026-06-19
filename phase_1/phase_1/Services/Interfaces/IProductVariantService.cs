using phase_1.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace phase_1.Services.Interfaces
{
    public interface IProductVariantService
    {
        Task<IEnumerable<ProductVariantDto>> GetVariantsByProductIdAsync(int productId);
        Task<ProductVariantDto?> GetVariantByIdAsync(int id);
        Task<ProductVariantDto> CreateVariantAsync(int productId, CreateProductVariantRequest request);
        Task<ProductVariantDto> UpdateVariantAsync(int id, UpdateProductVariantRequest request);
        Task<bool> DeleteVariantAsync(int id);
    }
}

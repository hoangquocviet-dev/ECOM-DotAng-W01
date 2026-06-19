using phase_1.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace phase_1.Repositories
{
    public interface IProductVariantRepository
    {
        Task<IEnumerable<ProductVariant>> GetVariantsByProductIdAsync(int productId);
        Task<ProductVariant?> GetByIdAsync(int id);
        Task AddAsync(ProductVariant variant);
        Task UpdateAsync(ProductVariant variant);
        Task DeleteAsync(ProductVariant variant);
        Task<ProductVariant?> GetBySkuAsync(string sku);
    }
}

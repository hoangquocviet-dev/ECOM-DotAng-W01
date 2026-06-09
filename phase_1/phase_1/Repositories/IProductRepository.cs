using phase_1.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace phase_1.Repositories
{
    public interface IProductRepository
    {
        Task<IEnumerable<Product>> GetAllAsync();
        Task<Product?> GetByIdAsync(int id);
        Task AddAsync(Product product);
        Task UpdateAsync(Product product);
        Task DeleteAsync(Product product);
        Task<IEnumerable<Product>> SearchProductsAsync(string keyword);
        Task<IEnumerable<Product>> GetTop3HighestPricedByCategoryAsync(string category);
        Task<IEnumerable<CategoryTotal>> GetTotalPriceByCategoryAsync();
        Task<phase_1.DTOs.PagedResult<Product>> GetProductsPagedAsync(int pageNumber, int pageSize, int? categoryId = null, int? brandId = null, decimal? minPrice = null, decimal? maxPrice = null, string keyword = "");
        Task<IEnumerable<Product>> GetFrequentlyBoughtTogetherAsync(int productId, int limit = 5);
    }
}
using phase_1.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace phase_1.Repositories
{
    public interface IProductImageRepository
    {
        Task<IEnumerable<ProductImage>> GetImagesByProductIdAsync(int productId);
        Task<ProductImage?> GetImageByIdAsync(int id);
        Task AddImageAsync(ProductImage image);
        Task DeleteImageAsync(ProductImage image);
    }
}

using phase_1.DTOs;
using phase_1.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace phase_1.Services.Interfaces
{
    public interface IProductImageService
    {
        Task<IEnumerable<ProductImage>> GetImagesByProductIdAsync(int productId);
        Task<ProductImage> AddImageAsync(CreateProductImageRequest request);
        Task<bool> DeleteImageAsync(int id);
    }
}

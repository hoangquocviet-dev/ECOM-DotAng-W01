using phase_1.DTOs;
using phase_1.Models;
using phase_1.Repositories;
using phase_1.Services.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace phase_1.Services
{
    public class ProductImageService : IProductImageService
    {
        private readonly IProductImageRepository _productImageRepository;

        public ProductImageService(IProductImageRepository productImageRepository)
        {
            _productImageRepository = productImageRepository;
        }

        public async Task<IEnumerable<ProductImage>> GetImagesByProductIdAsync(int productId)
        {
            return await _productImageRepository.GetImagesByProductIdAsync(productId);
        }

        public async Task<ProductImage> AddImageAsync(CreateProductImageRequest request)
        {
            var image = new ProductImage
            {
                ProductId = request.ProductId,
                ImageUrl = request.ImageUrl
            };

            await _productImageRepository.AddImageAsync(image);
            return image;
        }

        public async Task<bool> DeleteImageAsync(int id)
        {
            var image = await _productImageRepository.GetImageByIdAsync(id);
            if (image == null) return false;

            await _productImageRepository.DeleteImageAsync(image);
            return true;
        }
    }
}

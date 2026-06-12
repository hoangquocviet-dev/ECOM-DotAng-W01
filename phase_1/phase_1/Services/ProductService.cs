using phase_1.Models;
using phase_1.Repositories;
using phase_1.Services.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace phase_1.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;

        public ProductService(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<Product> CreateProductAsync(Product newProduct)
        {
            await _productRepository.AddAsync(newProduct);
            return newProduct;
        }

        public async Task<Product> UpdateProductAsync(Product updatedProduct)
        {
            await _productRepository.UpdateAsync(updatedProduct);
            return updatedProduct;
        }

        public async Task<Product> DeleteProductAsync(int id)
        {
            var product = await _productRepository.GetByIdAsync(id);
            if (product != null)
            {
                await _productRepository.DeleteAsync(product);
            }
            return product;
        }

        public async Task<IEnumerable<Product>> SearchProductsAsync(string keyword)
        {
            return await _productRepository.SearchProductsAsync(keyword);
        }

        public async Task<IEnumerable<Product>> GetTop3HighestPricedProductsByCategoryAsync(string category)
        {
            return await _productRepository.GetTop3HighestPricedByCategoryAsync(category);
        }

        public async Task<IEnumerable<CategoryTotal>> GetTotalPriceByCategoryAsync()
        {
            return await _productRepository.GetTotalPriceByCategoryAsync();
        }

        public async Task<phase_1.DTOs.PagedResult<Product>> GetProductsPagedAsync(int pageNumber, int pageSize, int? categoryId = null, int? brandId = null, decimal? minPrice = null, decimal? maxPrice = null, string keyword = "")
        {
            return await _productRepository.GetProductsPagedAsync(pageNumber, pageSize, categoryId, brandId, minPrice, maxPrice, keyword);
        }

        public async Task<IEnumerable<Product>> GetFrequentlyBoughtTogetherAsync(int productId, int limit = 5)
        {
            return await _productRepository.GetFrequentlyBoughtTogetherAsync(productId, limit);
        }

        public async Task<phase_1.DTOs.AutoSuggestDto> GetAutoSuggestAsync(string keyword, int limit = 5)
        {
            return await _productRepository.GetAutoSuggestAsync(keyword, limit);
        }
    }
}
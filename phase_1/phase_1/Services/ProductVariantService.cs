using phase_1.DTOs;
using phase_1.Models;
using phase_1.Repositories;
using phase_1.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace phase_1.Services
{
    public class ProductVariantService : IProductVariantService
    {
        private readonly IProductVariantRepository _variantRepository;
        private readonly IProductRepository _productRepository;

        public ProductVariantService(IProductVariantRepository variantRepository, IProductRepository productRepository)
        {
            _variantRepository = variantRepository;
            _productRepository = productRepository;
        }

        public async Task<IEnumerable<ProductVariantDto>> GetVariantsByProductIdAsync(int productId)
        {
            var variants = await _variantRepository.GetVariantsByProductIdAsync(productId);
            return variants.Select(v => new ProductVariantDto
            {
                Id = v.Id,
                ProductId = v.ProductId,
                SKU = v.SKU,
                Color = v.Color,
                Size = v.Size,
                Price = v.Price,
                StockQuantity = v.StockQuantity
            });
        }

        public async Task<ProductVariantDto?> GetVariantByIdAsync(int id)
        {
            var v = await _variantRepository.GetByIdAsync(id);
            if (v == null) return null;

            return new ProductVariantDto
            {
                Id = v.Id,
                ProductId = v.ProductId,
                SKU = v.SKU,
                Color = v.Color,
                Size = v.Size,
                Price = v.Price,
                StockQuantity = v.StockQuantity
            };
        }

        public async Task<ProductVariantDto> CreateVariantAsync(int productId, CreateProductVariantRequest request)
        {
            var product = await _productRepository.GetByIdAsync(productId);
            if (product == null)
                throw new Exception("Product not found");

            var existingSku = await _variantRepository.GetBySkuAsync(request.SKU);
            if (existingSku != null)
                throw new Exception("SKU already exists");

            var variant = new ProductVariant
            {
                ProductId = productId,
                SKU = request.SKU,
                Color = request.Color,
                Size = request.Size,
                Price = request.Price,
                StockQuantity = request.StockQuantity
            };

            await _variantRepository.AddAsync(variant);

            return new ProductVariantDto
            {
                Id = variant.Id,
                ProductId = variant.ProductId,
                SKU = variant.SKU,
                Color = variant.Color,
                Size = variant.Size,
                Price = variant.Price,
                StockQuantity = variant.StockQuantity
            };
        }

        public async Task<ProductVariantDto> UpdateVariantAsync(int id, UpdateProductVariantRequest request)
        {
            var variant = await _variantRepository.GetByIdAsync(id);
            if (variant == null)
                throw new Exception("Variant not found");

            if (!string.IsNullOrEmpty(request.SKU) && request.SKU != variant.SKU)
            {
                var existingSku = await _variantRepository.GetBySkuAsync(request.SKU);
                if (existingSku != null)
                    throw new Exception("SKU already exists");
                variant.SKU = request.SKU;
            }

            if (request.Color != null) variant.Color = request.Color;
            if (request.Size != null) variant.Size = request.Size;
            if (request.Price.HasValue) variant.Price = request.Price.Value;
            if (request.StockQuantity.HasValue) variant.StockQuantity = request.StockQuantity.Value;

            await _variantRepository.UpdateAsync(variant);

            return new ProductVariantDto
            {
                Id = variant.Id,
                ProductId = variant.ProductId,
                SKU = variant.SKU,
                Color = variant.Color,
                Size = variant.Size,
                Price = variant.Price,
                StockQuantity = variant.StockQuantity
            };
        }

        public async Task<bool> DeleteVariantAsync(int id)
        {
            var variant = await _variantRepository.GetByIdAsync(id);
            if (variant == null) return false;

            await _variantRepository.DeleteAsync(variant);
            return true;
        }
    }
}

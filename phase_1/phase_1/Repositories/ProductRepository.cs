using Microsoft.EntityFrameworkCore;
using phase_1.Data;
using phase_1.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace phase_1.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly ApplicationDbContext _context;
        public ProductRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Product>> GetAllAsync()
        {
            return await _context.Products.ToListAsync();
        }

        public async Task<Product?> GetByIdAsync(int id)
        {
            return await _context.Products
                .Include(p => p.ProductVariants)
                .Include(p => p.ProductImages)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task AddAsync(Product product)
        {
            await _context.Products.AddAsync(product);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Product product)
        {
            _context.Products.Update(product);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Product product)
        {
            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
        }
        public async Task<IEnumerable<Product>> SearchProductsAsync(string keyword)
        {
            return await _context.Products
                .Include(p => p.Category)
                .Include(p => p.ProductImages)
                .Include(p => p.ProductVariants)
                .Where(p => p.Name.Contains(keyword))
                .ToListAsync();
        }

        public async Task<IEnumerable<Product>> GetTop3HighestPricedByCategoryAsync(string category)
        {
            return await _context.Products
                .Include(p => p.Category)
                .Where(p => p.Category != null && p.Category.Name == category)
                .OrderByDescending(p => p.Price)
                .Take(3)
                .ToListAsync();
        }

        public async Task<IEnumerable<CategoryTotal>> GetTotalPriceByCategoryAsync()
        {
            return await _context.Categories
                .Select(c => new CategoryTotal
                {
                    Category = c.Name,
                    TotalValue = c.Products.Sum(p => p.Price)
                })
                .ToListAsync();
        }

        public async Task<phase_1.DTOs.PagedResult<Product>> GetProductsPagedAsync(int pageNumber, int pageSize, int? categoryId = null, int? brandId = null, decimal? minPrice = null, decimal? maxPrice = null, string keyword = "")
        {
            var query = _context.Products
                .Include(p => p.Category)
                .Include(p => p.Brand)
                .Include(p => p.ProductImages)
                .Include(p => p.ProductVariants)
                .AsQueryable();

            if (categoryId.HasValue)
                query = query.Where(p => p.CategoryId == categoryId.Value);

            if (brandId.HasValue)
                query = query.Where(p => p.BrandId == brandId.Value);

            if (minPrice.HasValue)
                query = query.Where(p => p.Price >= minPrice.Value);

            if (maxPrice.HasValue)
                query = query.Where(p => p.Price <= maxPrice.Value);

            if (!string.IsNullOrEmpty(keyword))
                query = query.Where(p => p.Name.Contains(keyword));

            var totalItems = await query.CountAsync();
            var items = await query
                .OrderByDescending(p => p.Id)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new phase_1.DTOs.PagedResult<Product>
            {
                TotalItems = totalItems,
                PageNumber = pageNumber,
                PageSize = pageSize,
                Items = items
            };
        }

        public async Task<IEnumerable<Product>> GetFrequentlyBoughtTogetherAsync(int productId, int limit = 5)
        {
            var relatedProductIds = await _context.OrderDetails
                .Where(od => _context.OrderDetails
                    .Where(o => o.ProductId == productId)
                    .Select(o => o.OrderId)
                    .Contains(od.OrderId)
                    && od.ProductId != productId)
                .GroupBy(od => od.ProductId)
                .OrderByDescending(g => g.Count())
                .Select(g => g.Key)
                .Take(limit)
                .ToListAsync();

            return await _context.Products
                .Include(p => p.Category)
                .Include(p => p.Brand)
                .Include(p => p.ProductImages)
                .Include(p => p.ProductVariants)
                .Where(p => relatedProductIds.Contains(p.Id))
                .ToListAsync();
        }

        public async Task<phase_1.DTOs.AutoSuggestDto> GetAutoSuggestAsync(string keyword, int limit = 5)
        {
            if (string.IsNullOrWhiteSpace(keyword))
            {
                return new phase_1.DTOs.AutoSuggestDto();
            }

            var lowerKeyword = keyword.ToLower();

            var productsQuery = _context.Products
                .Include(p => p.Category)
                .Include(p => p.Brand)
                .Where(p => p.Name.ToLower().Contains(lowerKeyword))
                .Take(limit);

            var products = await productsQuery.Select(p => new phase_1.DTOs.ProductSuggestDto
            {
                Id = p.Id,
                Name = p.Name,
                Price = p.Price,
                ImageUrl = p.ImageUrl,
                CategoryName = p.Category != null ? p.Category.Name : string.Empty,
                BrandName = p.Brand != null ? p.Brand.Name : string.Empty
            }).ToListAsync();

            var categories = await _context.Categories
                .Where(c => c.Name.ToLower().Contains(lowerKeyword))
                .Select(c => c.Name)
                .Take(3)
                .ToListAsync();

            var brands = await _context.Brands
                .Where(b => b.Name.ToLower().Contains(lowerKeyword))
                .Select(b => b.Name)
                .Take(3)
                .ToListAsync();

            var keywords = new List<string>();
            keywords.AddRange(categories);
            keywords.AddRange(brands);

            return new phase_1.DTOs.AutoSuggestDto
            {
                Products = products,
                Keywords = keywords.Distinct().Take(limit)
            };
        }
    }
}
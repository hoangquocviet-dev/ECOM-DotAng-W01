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
            return await _context.Products.FindAsync(id);
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
            // Tìm kiếm tương đối theo tên
            return await _context.Products
                .Where(p => p.Name.Contains(keyword))
                .ToListAsync();
        }

        public async Task<IEnumerable<Product>> GetTop3HighestPricedByCategoryAsync(string category)
        {
            return await _context.Products
                .Where(p => p.Category == category)
                .OrderByDescending(p => p.Price)
                .Take(3)
                .ToListAsync();
        }

        public async Task<IEnumerable<CategoryTotal>> GetTotalPriceByCategoryAsync()
        {
            return await _context.Products
                .GroupBy(p => p.Category)
                .Select(g => new CategoryTotal
                {
                    Category = g.Key,
                    TotalValue = g.Sum(p => p.Price)
                })
                .ToListAsync();
        }
    }
}
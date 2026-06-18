using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using phase_1.Data;
using phase_1.Models;

namespace phase_1.Repositories
{
    public class FlashSaleRepository : IFlashSaleRepository
    {
        private readonly ApplicationDbContext _context;

        public FlashSaleRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<FlashSale>> GetAllAsync()
        {
            return await _context.FlashSales
                .Include(fs => fs.FlashSaleItems)
                .ThenInclude(fsi => fsi.Product)
                .OrderByDescending(fs => fs.StartTime)
                .ToListAsync();
        }

        public async Task<FlashSale?> GetByIdAsync(int id)
        {
            return await _context.FlashSales
                .Include(fs => fs.FlashSaleItems)
                .ThenInclude(fsi => fsi.Product)
                .FirstOrDefaultAsync(fs => fs.Id == id);
        }

        public async Task<FlashSale?> GetActiveFlashSaleAsync()
        {
            var now = DateTime.UtcNow;
            return await _context.FlashSales
                .Include(fs => fs.FlashSaleItems)
                .ThenInclude(fsi => fsi.Product)
                .Where(fs => fs.IsActive && fs.StartTime <= now && fs.EndTime >= now)
                .OrderBy(fs => fs.EndTime)
                .FirstOrDefaultAsync();
        }

        public async Task<FlashSale> CreateAsync(FlashSale flashSale)
        {
            _context.FlashSales.Add(flashSale);
            await _context.SaveChangesAsync();
            return flashSale;
        }

        public async Task UpdateAsync(FlashSale flashSale)
        {
            _context.FlashSales.Update(flashSale);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var flashSale = await _context.FlashSales.FindAsync(id);
            if (flashSale != null)
            {
                _context.FlashSales.Remove(flashSale);
                await _context.SaveChangesAsync();
            }
        }
    }
}

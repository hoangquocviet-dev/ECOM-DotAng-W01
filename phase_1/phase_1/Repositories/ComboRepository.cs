using Microsoft.EntityFrameworkCore;
using phase_1.Data;
using phase_1.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace phase_1.Repositories
{
    public class ComboRepository : IComboRepository
    {
        private readonly ApplicationDbContext _context;

        public ComboRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Combo>> GetAllAsync()
        {
            return await _context.Combos
                .Include(c => c.ComboItems)
                .ThenInclude(ci => ci.Product)
                .OrderByDescending(c => c.CreatedAt)
                .ToListAsync();
        }

        public async Task<IEnumerable<Combo>> GetAllActiveAsync()
        {
            return await _context.Combos
                .Include(c => c.ComboItems)
                .ThenInclude(ci => ci.Product)
                .Where(c => c.IsActive)
                .OrderByDescending(c => c.CreatedAt)
                .ToListAsync();
        }

        public async Task<Combo?> GetByIdAsync(int id)
        {
            return await _context.Combos
                .Include(c => c.ComboItems)
                .ThenInclude(ci => ci.Product)
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<Combo> AddAsync(Combo combo)
        {
            _context.Combos.Add(combo);
            await _context.SaveChangesAsync();
            return combo;
        }

        public async Task<Combo> UpdateAsync(Combo combo)
        {
            _context.Combos.Update(combo);
            await _context.SaveChangesAsync();
            return combo;
        }

        public async Task DeleteAsync(Combo combo)
        {
            _context.Combos.Remove(combo);
            await _context.SaveChangesAsync();
        }
    }
}

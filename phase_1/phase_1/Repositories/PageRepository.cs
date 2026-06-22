using Microsoft.EntityFrameworkCore;
using phase_1.Data;
using phase_1.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace phase_1.Repositories
{
    public class PageRepository : IPageRepository
    {
        private readonly ApplicationDbContext _context;

        public PageRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Page>> GetAllAsync(bool includeInactive = false)
        {
            if (includeInactive)
            {
                return await _context.Pages.ToListAsync();
            }
            return await _context.Pages.Where(p => p.IsActive).ToListAsync();
        }

        public async Task<Page?> GetByIdAsync(int id)
        {
            return await _context.Pages.FindAsync(id);
        }

        public async Task<Page?> GetBySlugAsync(string slug)
        {
            return await _context.Pages.FirstOrDefaultAsync(p => p.Slug == slug);
        }

        public async Task AddAsync(Page page)
        {
            await _context.Pages.AddAsync(page);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Page page)
        {
            _context.Pages.Update(page);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Page page)
        {
            _context.Pages.Remove(page);
            await _context.SaveChangesAsync();
        }
    }
}

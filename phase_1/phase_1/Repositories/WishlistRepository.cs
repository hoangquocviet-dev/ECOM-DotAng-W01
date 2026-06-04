using Microsoft.EntityFrameworkCore;
using phase_1.Data;
using phase_1.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace phase_1.Repositories
{
    public class WishlistRepository : IWishlistRepository
    {
        private readonly ApplicationDbContext _context;

        public WishlistRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<WishlistItem>> GetWishlistByUserIdAsync(int userId)
        {
            return await _context.WishlistItems
                .Include(w => w.Product)
                .Where(w => w.UserId == userId)
                .OrderByDescending(w => w.DateAdded)
                .ToListAsync();
        }

        public async Task<WishlistItem?> GetWishlistItemAsync(int userId, int productId)
        {
            return await _context.WishlistItems
                .FirstOrDefaultAsync(w => w.UserId == userId && w.ProductId == productId);
        }

        public async Task AddToWishlistAsync(WishlistItem item)
        {
            await _context.WishlistItems.AddAsync(item);
            await _context.SaveChangesAsync();
        }

        public async Task RemoveFromWishlistAsync(WishlistItem item)
        {
            _context.WishlistItems.Remove(item);
            await _context.SaveChangesAsync();
        }
    }
}

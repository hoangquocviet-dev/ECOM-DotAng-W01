using phase_1.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace phase_1.Repositories
{
    public interface IWishlistRepository
    {
        Task<IEnumerable<WishlistItem>> GetWishlistByUserIdAsync(int userId);
        Task<WishlistItem?> GetWishlistItemAsync(int userId, int productId);
        Task AddToWishlistAsync(WishlistItem item);
        Task RemoveFromWishlistAsync(WishlistItem item);
    }
}

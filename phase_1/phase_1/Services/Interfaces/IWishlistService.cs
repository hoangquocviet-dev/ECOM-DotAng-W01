using phase_1.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace phase_1.Services.Interfaces
{
    public interface IWishlistService
    {
        Task<IEnumerable<WishlistItem>> GetUserWishlistAsync(int userId);
        Task<bool> AddToWishlistAsync(int userId, int productId);
        Task<bool> RemoveFromWishlistAsync(int userId, int productId);
    }
}

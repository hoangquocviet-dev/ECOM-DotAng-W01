using phase_1.Models;
using System.Threading.Tasks;

namespace phase_1.Services.Interfaces
{
    public interface ICartService
    {
        Task<Cart> GetCartAsync(int userId);
        Task<Cart> AddToCartAsync(int userId, int productId, int quantity);
        Task<Cart> UpdateCartItemAsync(int userId, int cartItemId, int quantity);
        Task<Cart> RemoveFromCartAsync(int userId, int cartItemId);
    }
}

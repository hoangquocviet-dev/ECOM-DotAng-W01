using phase_1.Models;
using System.Threading.Tasks;

namespace phase_1.Repositories
{
    public interface ICartRepository
    {
        Task<Cart?> GetCartByUserIdAsync(int userId);
        Task<Cart> CreateCartAsync(Cart cart);
        Task<CartItem?> GetCartItemAsync(int cartId, int productId);
        Task<CartItem?> GetCartItemByIdAsync(int cartItemId);
        Task AddCartItemAsync(CartItem item);
        Task UpdateCartItemAsync(CartItem item);
        Task RemoveCartItemAsync(CartItem item);
        Task UpdateCartAsync(Cart cart);
        Task<System.Collections.Generic.IEnumerable<Cart>> GetAbandonedCartsAsync(System.DateTime thresholdDate);
    }
}

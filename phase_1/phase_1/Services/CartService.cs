using phase_1.Models;
using phase_1.Repositories;
using phase_1.Services.Interfaces;
using System.Threading.Tasks;

namespace phase_1.Services
{
    public class CartService : ICartService
    {
        private readonly ICartRepository _cartRepository;

        public CartService(ICartRepository cartRepository)
        {
            _cartRepository = cartRepository;
        }

        public async Task<Cart> GetCartAsync(int userId)
        {
            var cart = await _cartRepository.GetCartByUserIdAsync(userId);
            if (cart == null)
            {
                cart = await _cartRepository.CreateCartAsync(new Cart { UserId = userId });
            }
            return cart;
        }

        public async Task<Cart> AddToCartAsync(int userId, int productId, int quantity)
        {
            var cart = await GetCartAsync(userId);
            var existingItem = await _cartRepository.GetCartItemAsync(cart.Id, productId);

            if (existingItem != null)
            {
                existingItem.Quantity += quantity;
                await _cartRepository.UpdateCartItemAsync(existingItem);
            }
            else
            {
                var newItem = new CartItem
                {
                    CartId = cart.Id,
                    ProductId = productId,
                    Quantity = quantity
                };
                await _cartRepository.AddCartItemAsync(newItem);
            }
            cart.UpdatedAt = System.DateTime.UtcNow;
            cart.IsReminderSent = false;
            await _cartRepository.UpdateCartAsync(cart);

            return await _cartRepository.GetCartByUserIdAsync(userId) ?? cart;
        }

        public async Task<Cart> UpdateCartItemAsync(int userId, int cartItemId, int quantity)
        {
            var cart = await GetCartAsync(userId);
            var item = await _cartRepository.GetCartItemByIdAsync(cartItemId);
            
            if (item != null && item.CartId == cart.Id)
            {
                item.Quantity = quantity;
                await _cartRepository.UpdateCartItemAsync(item);
                
                cart.UpdatedAt = System.DateTime.UtcNow;
                cart.IsReminderSent = false;
                await _cartRepository.UpdateCartAsync(cart);
            }
            return await _cartRepository.GetCartByUserIdAsync(userId) ?? cart;
        }

        public async Task<Cart> RemoveFromCartAsync(int userId, int cartItemId)
        {
            var cart = await GetCartAsync(userId);
            var item = await _cartRepository.GetCartItemByIdAsync(cartItemId);

            if (item != null && item.CartId == cart.Id)
            {
                await _cartRepository.RemoveCartItemAsync(item);
                
                cart.UpdatedAt = System.DateTime.UtcNow;
                cart.IsReminderSent = false;
                await _cartRepository.UpdateCartAsync(cart);
            }
            return await _cartRepository.GetCartByUserIdAsync(userId) ?? cart;
        }
    }
}

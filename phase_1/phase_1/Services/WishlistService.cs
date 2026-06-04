using phase_1.Models;
using phase_1.Repositories;
using phase_1.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace phase_1.Services
{
    public class WishlistService : IWishlistService
    {
        private readonly IWishlistRepository _wishlistRepository;
        private readonly IProductRepository _productRepository;

        public WishlistService(IWishlistRepository wishlistRepository, IProductRepository productRepository)
        {
            _wishlistRepository = wishlistRepository;
            _productRepository = productRepository;
        }

        public async Task<IEnumerable<WishlistItem>> GetUserWishlistAsync(int userId)
        {
            return await _wishlistRepository.GetWishlistByUserIdAsync(userId);
        }

        public async Task<bool> AddToWishlistAsync(int userId, int productId)
        {
            // Check if product exists
            var product = await _productRepository.GetByIdAsync(productId);
            if (product == null)
            {
                return false;
            }

            // Check if already in wishlist
            var existingItem = await _wishlistRepository.GetWishlistItemAsync(userId, productId);
            if (existingItem != null)
            {
                return true; // Already added
            }

            var newItem = new WishlistItem
            {
                UserId = userId,
                ProductId = productId,
                DateAdded = DateTime.UtcNow
            };

            await _wishlistRepository.AddToWishlistAsync(newItem);
            return true;
        }

        public async Task<bool> RemoveFromWishlistAsync(int userId, int productId)
        {
            var item = await _wishlistRepository.GetWishlistItemAsync(userId, productId);
            if (item == null)
            {
                return false;
            }

            await _wishlistRepository.RemoveFromWishlistAsync(item);
            return true;
        }
    }
}

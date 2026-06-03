using phase_1.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace phase_1.Services.Interfaces
{
    public interface IOrderService
    {
        Task<Order?> CheckoutAsync(int userId, string shippingAddress);
        Task<IEnumerable<Order>> GetOrdersByUserIdAsync(int userId);
    }
}

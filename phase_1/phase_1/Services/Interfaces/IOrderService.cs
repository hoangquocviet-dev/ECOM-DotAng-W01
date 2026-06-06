using phase_1.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace phase_1.Services.Interfaces
{
    public interface IOrderService
    {
        Task<Order?> CheckoutAsync(int userId, string shippingAddress, string? voucherCode = null);
        Task<IEnumerable<Order>> GetOrdersByUserIdAsync(int userId);
        Task<IEnumerable<Order>> GetAllOrdersAsync();
        Task<Order?> UpdateOrderStatusAsync(int orderId, string newStatus);
    }
}

using phase_1.Models;
using phase_1.Repositories;
using phase_1.Services.Interfaces;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace phase_1.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly ICartRepository _cartRepository;
        private readonly IProductRepository _productRepository;
        private readonly IVoucherRepository _voucherRepository;

        public OrderService(IOrderRepository orderRepository, ICartRepository cartRepository, IProductRepository productRepository, IVoucherRepository voucherRepository)
        {
            _orderRepository = orderRepository;
            _cartRepository = cartRepository;
            _productRepository = productRepository;
            _voucherRepository = voucherRepository;
        }

        public async Task<Order?> CheckoutAsync(int userId, string shippingAddress, string? voucherCode = null)
        {
            var cart = await _cartRepository.GetCartByUserIdAsync(userId);
            if (cart == null || !cart.CartItems.Any())
            {
                return null;
            }

            var order = new Order
            {
                UserId = userId,
                ShippingAddress = shippingAddress,
                OrderDate = DateTime.UtcNow,
                Status = "Pending",
                TotalAmount = 0
            };

            foreach (var item in cart.CartItems)
            {
                var product = await _productRepository.GetByIdAsync(item.ProductId);
                if (product != null)
                {
                    if (product.StockQuantity >= item.Quantity)
                    {
                        product.StockQuantity -= item.Quantity;
                        await _productRepository.UpdateAsync(product);
                    }

                    var orderDetail = new OrderDetail
                    {
                        ProductId = item.ProductId,
                        Quantity = item.Quantity,
                        UnitPrice = product.Price
                    };
                    order.OrderDetails.Add(orderDetail);
                    order.TotalAmount += (orderDetail.Quantity * orderDetail.UnitPrice);
                }
            }

            if (!string.IsNullOrEmpty(voucherCode))
            {
                var voucher = await _voucherRepository.GetVoucherByCodeAsync(voucherCode);
                if (voucher != null && voucher.ExpiryDate > DateTime.UtcNow && voucher.UsedCount < voucher.UsageLimit)
                {
                    order.VoucherId = voucher.Id;
                    order.DiscountAmount = voucher.DiscountAmount;
                    order.TotalAmount -= voucher.DiscountAmount;
                    if (order.TotalAmount < 0) order.TotalAmount = 0;

                    voucher.UsedCount++;
                    await _voucherRepository.UpdateVoucherAsync(voucher);
                }
                else
                {
                    // Invalid or expired voucher, but for simplicity we can either throw exception or just not apply it.
                    // We'll just throw an ArgumentException to be handled by controller, or just ignore.
                    throw new ArgumentException("Mã giảm giá không hợp lệ, đã hết hạn hoặc hết lượt sử dụng.");
                }
            }

            var createdOrder = await _orderRepository.CreateOrderAsync(order);

            // Clear Cart
            foreach (var item in cart.CartItems.ToList())
            {
                await _cartRepository.RemoveCartItemAsync(item);
            }

            return createdOrder;
        }

        public async Task<IEnumerable<Order>> GetOrdersByUserIdAsync(int userId)
        {
            return await _orderRepository.GetOrdersByUserIdAsync(userId);
        }

        public async Task<IEnumerable<Order>> GetAllOrdersAsync()
        {
            return await _orderRepository.GetAllOrdersAsync();
        }

        public async Task<Order?> UpdateOrderStatusAsync(int orderId, string newStatus)
        {
            var order = await _orderRepository.GetOrderByIdAsync(orderId);
            if (order == null)
            {
                return null;
            }

            order.Status = newStatus;
            await _orderRepository.UpdateOrderAsync(order);
            return order;
        }
    }
}

using phase_1.Models;
using phase_1.Repositories;
using phase_1.Services.Interfaces;
using System;
using System.Linq;
using System.Threading.Tasks;

using phase_1.Hubs;
using Microsoft.AspNetCore.SignalR;

namespace phase_1.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly ICartRepository _cartRepository;
        private readonly IProductRepository _productRepository;
        private readonly IVoucherRepository _voucherRepository;
        private readonly IUserRepository _userRepository;
        private readonly IHubContext<NotificationHub> _hubContext;

        public OrderService(IOrderRepository orderRepository, ICartRepository cartRepository, IProductRepository productRepository, IVoucherRepository voucherRepository, IUserRepository userRepository, IHubContext<NotificationHub> hubContext)
        {
            _orderRepository = orderRepository;
            _cartRepository = cartRepository;
            _productRepository = productRepository;
            _voucherRepository = voucherRepository;
            _userRepository = userRepository;
            _hubContext = hubContext;
        }

        public async Task<Order?> CheckoutAsync(int userId, string shippingAddress, string paymentMethod, string? voucherCode = null)
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
                TotalAmount = 0,
                PaymentMethod = paymentMethod,
                PaymentStatus = "Pending"
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
                    throw new ArgumentException("Mã giảm giá không hợp lệ, đã hết hạn hoặc hết lượt sử dụng.");
                }
            }

            var createdOrder = await _orderRepository.CreateOrderAsync(order);

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

            if (newStatus == "Completed" && order.Status != "Completed")
            {
                var user = await _userRepository.GetUserByIdAsync(order.UserId);
                if (user != null)
                {
                    user.TotalSpent += order.TotalAmount;
                    user.RewardPoints += (int)(order.TotalAmount / 100); 
                    
                    if (user.TotalSpent >= 10000000) user.MemberTier = "VIP";
                    else if (user.TotalSpent >= 5000000) user.MemberTier = "Gold";
                    else if (user.TotalSpent >= 2000000) user.MemberTier = "Silver";
                    else user.MemberTier = "Bronze";

                    await _userRepository.UpdateUserAsync(user);
                }
            }

            order.Status = newStatus;
            await _orderRepository.UpdateOrderAsync(order);
            
            await _hubContext.Clients.All.SendAsync("ReceiveOrderStatusUpdate", order.Id, order.Status, order.UserId);

            return order;
        }

        public async Task<Order?> UpdatePaymentStatusAsync(int orderId, string paymentStatus)
        {
            var order = await _orderRepository.GetOrderByIdAsync(orderId);
            if (order == null)
            {
                return null;
            }

            order.PaymentStatus = paymentStatus;
            if (paymentStatus == "Paid")
            {
                order.Status = "Processing";
            }
            
            await _orderRepository.UpdateOrderAsync(order);
            return order;
        }
    }
}

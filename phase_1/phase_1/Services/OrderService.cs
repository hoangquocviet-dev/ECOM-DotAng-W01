using phase_1.Models;
using phase_1.Repositories;
using phase_1.Services.Interfaces;
using System;
using System.Linq;
using System.Threading.Tasks;

using phase_1.Hubs;
using Microsoft.AspNetCore.SignalR;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

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
        private readonly ICompanySettingRepository _companySettingRepository;

        public OrderService(IOrderRepository orderRepository, ICartRepository cartRepository, IProductRepository productRepository, IVoucherRepository voucherRepository, IUserRepository userRepository, IHubContext<NotificationHub> hubContext, ICompanySettingRepository companySettingRepository)
        {
            _orderRepository = orderRepository;
            _cartRepository = cartRepository;
            _productRepository = productRepository;
            _voucherRepository = voucherRepository;
            _userRepository = userRepository;
            _hubContext = hubContext;
            _companySettingRepository = companySettingRepository;
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

                        if (product.StockQuantity < product.LowStockThreshold)
                        {
                            await _hubContext.Clients.All.SendAsync("ReceiveLowStockAlert", product.Id, product.Name, product.StockQuantity);
                        }
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

            if (newStatus == "Cancelled" && order.Status != "Cancelled")
            {
                foreach (var detail in order.OrderDetails)
                {
                    if (detail.Product != null)
                    {
                        detail.Product.StockQuantity += detail.Quantity;
                        await _productRepository.UpdateAsync(detail.Product);
                    }
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
            else if (paymentStatus == "Failed" && order.Status != "Cancelled")
            {
                order.Status = "Cancelled";
                foreach (var detail in order.OrderDetails)
                {
                    if (detail.Product != null)
                    {
                        detail.Product.StockQuantity += detail.Quantity;
                        await _productRepository.UpdateAsync(detail.Product);
                    }
                }
            }
            
            await _orderRepository.UpdateOrderAsync(order);
            return order;
        }

        public async Task<byte[]> GenerateInvoicePdfAsync(int orderId)
        {
            var order = await _orderRepository.GetOrderByIdAsync(orderId);
            if (order == null) throw new ArgumentException("Order not found");

            var companySettings = await _companySettingRepository.GetSettingsAsync();
            
            var document = Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4);
                    page.Margin(2, Unit.Centimetre);
                    page.PageColor(Colors.White);
                    page.DefaultTextStyle(x => x.FontSize(12));

                    page.Header().Element(header => ComposeHeader(header, companySettings, order));
                    page.Content().Element(content => ComposeContent(content, order));
                    page.Footer().AlignCenter().Text(x =>
                    {
                        x.CurrentPageNumber();
                        x.Span(" / ");
                        x.TotalPages();
                    });
                });
            });

            return document.GeneratePdf();
        }

        private void ComposeHeader(IContainer container, CompanySetting company, Order order)
        {
            container.Row(row =>
            {
                row.RelativeItem().Column(column =>
                {
                    column.Item().Text(company?.CompanyName ?? "E-Commerce Store").FontSize(20).SemiBold().FontColor(Colors.Blue.Darken2);
                    column.Item().Text(company?.Address ?? "Hanoi, Vietnam");
                    column.Item().Text(company?.Hotline ?? "0886528046");
                    column.Item().Text(company?.Email ?? "hoangquocviet.dev@gmail.com");
                });
            });
        }

        private void ComposeContent(IContainer container, Order order)
        {
            container.PaddingVertical(1, Unit.Centimetre).Column(column =>
            {
                column.Spacing(5);
                column.Item().Text("HÓA ĐƠN MUA HÀNG").FontSize(24).SemiBold().FontColor(Colors.Blue.Darken2).AlignCenter();
                
                column.Item().Row(row =>
                {
                    row.RelativeItem().Column(col =>
                    {
                        col.Item().Text($"Mã hóa đơn: #{order.Id}").SemiBold();
                        col.Item().Text($"Ngày mua: {order.OrderDate:dd/MM/yyyy HH:mm}");
                        col.Item().Text($"Khách hàng: {order.User?.Name ?? "Khách hàng"}");
                        col.Item().Text($"Số điện thoại: {order.User?.PhoneNumber ?? "Không có"}");
                        col.Item().Text($"Địa chỉ giao hàng: {order.ShippingAddress}");
                    });
                });

                column.Item().PaddingTop(25).Element(tableContainer => ComposeTable(tableContainer, order));

                var totalPrice = order.TotalAmount + order.DiscountAmount;
                
                column.Item().PaddingTop(25).AlignRight().Column(col => 
                {
                    col.Item().Text($"Tổng tiền: {totalPrice:C}").FontSize(14);
                    if (order.DiscountAmount > 0)
                    {
                        col.Item().Text($"Giảm giá: -{order.DiscountAmount:C}").FontSize(14).FontColor(Colors.Red.Medium);
                    }
                    col.Item().Text($"Thành tiền: {order.TotalAmount:C}").FontSize(16).SemiBold();
                    col.Item().Text($"Phương thức thanh toán: {order.PaymentMethod}");
                    col.Item().Text($"Trạng thái thanh toán: {order.PaymentStatus}");
                });
            });
        }

        private void ComposeTable(IContainer container, Order order)
        {
            container.Table(table =>
            {
                table.ColumnsDefinition(columns =>
                {
                    columns.ConstantColumn(25);
                    columns.RelativeColumn(3);
                    columns.RelativeColumn();
                    columns.RelativeColumn();
                    columns.RelativeColumn();
                });

                table.Header(header =>
                {
                    header.Cell().Text("#").SemiBold();
                    header.Cell().Text("Sản phẩm").SemiBold();
                    header.Cell().AlignRight().Text("Đơn giá").SemiBold();
                    header.Cell().AlignRight().Text("Số lượng").SemiBold();
                    header.Cell().AlignRight().Text("Thành tiền").SemiBold();

                    header.Cell().ColumnSpan(5).PaddingVertical(5).BorderBottom(1).BorderColor(Colors.Black);
                });

                var index = 1;
                foreach (var item in order.OrderDetails)
                {
                    table.Cell().Element(CellStyle).Text(index.ToString());
                    table.Cell().Element(CellStyle).Text(item.Product?.Name ?? "Sản phẩm");
                    table.Cell().Element(CellStyle).AlignRight().Text($"{item.UnitPrice:C}");
                    table.Cell().Element(CellStyle).AlignRight().Text(item.Quantity.ToString());
                    table.Cell().Element(CellStyle).AlignRight().Text($"{item.UnitPrice * item.Quantity:C}");

                    static IContainer CellStyle(IContainer container)
                    {
                        return container.BorderBottom(1).BorderColor(Colors.Grey.Lighten2).PaddingVertical(5);
                    }
                    index++;
                }
            });
        }
    }
}

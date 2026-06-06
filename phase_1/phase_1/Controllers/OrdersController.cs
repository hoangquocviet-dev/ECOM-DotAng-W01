using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using phase_1.DTOs;
using phase_1.Services.Interfaces;
using System.Security.Claims;
using System.Threading.Tasks;

namespace phase_1.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderService _orderService;
        private readonly IMomoService _momoService;

        public OrdersController(IOrderService orderService, IMomoService momoService)
        {
            _orderService = orderService;
            _momoService = momoService;
        }

        private int GetUserId()
        {
            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (int.TryParse(userIdString, out int userId))
            {
                return userId;
            }
            return 0;
        }

        [HttpPost("checkout")]
        public async Task<IActionResult> CheckoutAsync([FromBody] CheckoutRequest request)
        {
            try
            {
                int userId = GetUserId();
                var order = await _orderService.CheckoutAsync(userId, request.ShippingAddress, request.PaymentMethod, request.VoucherCode);
                if (order == null)
                {
                    return BadRequest("Cart is empty or invalid.");
                }
                
                if (request.PaymentMethod == "MoMo")
                {
                    var paymentUrl = await _momoService.CreatePaymentUrl(order);
                    return Ok(new { order, paymentUrl });
                }

                return Ok(new { order, paymentUrl = "" });
            }
            catch (System.ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("history")]
        public async Task<IActionResult> GetOrderHistoryAsync()
        {
            int userId = GetUserId();
            var orders = await _orderService.GetOrdersByUserIdAsync(userId);
            return Ok(orders);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> GetAllOrdersAsync()
        {
            var orders = await _orderService.GetAllOrdersAsync();
            return Ok(orders);
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("{id}/status")]
        public async Task<IActionResult> UpdateOrderStatusAsync(int id, [FromBody] UpdateOrderStatusRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var order = await _orderService.UpdateOrderStatusAsync(id, request.Status);
            if (order == null)
            {
                return NotFound($"Order with ID {id} not found.");
            }

            return Ok(order);
        }

        [AllowAnonymous]
        [HttpGet("momo-return")]
        public IActionResult MomoReturn([FromQuery] string partnerCode, [FromQuery] string orderId, [FromQuery] string requestId, [FromQuery] string amount, [FromQuery] string orderInfo, [FromQuery] string orderType, [FromQuery] string transId, [FromQuery] string resultCode, [FromQuery] string message, [FromQuery] string payType, [FromQuery] string responseTime, [FromQuery] string extraData, [FromQuery] string signature)
        {
            if (resultCode == "0")
            {
                return Ok(new { message = "Thanh toán thành công qua MoMo", orderId });
            }
            return BadRequest(new { message = "Thanh toán thất bại", orderId });
        }

        [AllowAnonymous]
        [HttpPost("momo-notify")]
        public IActionResult MomoNotify([FromBody] object ipnData)
        {
            // In a real application, we would validate signature and update payment status here
            return NoContent();
        }
    }
}

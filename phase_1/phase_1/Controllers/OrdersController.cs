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

        public OrdersController(IOrderService orderService)
        {
            _orderService = orderService;
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
                var order = await _orderService.CheckoutAsync(userId, request.ShippingAddress, request.VoucherCode);
                if (order == null)
                {
                    return BadRequest("Cart is empty or invalid.");
                }
                return Ok(order);
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
    }
}

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

        [HttpGet("{id}/invoice")]
        public async Task<IActionResult> ExportInvoiceAsync(int id)
        {
            try
            {
                var pdfBytes = await _orderService.GenerateInvoicePdfAsync(id);
                return File(pdfBytes, "application/pdf", $"Invoice_{id}.pdf");
            }
            catch (System.ArgumentException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [AllowAnonymous]
        [HttpGet("momo-return")]
        public async Task<IActionResult> MomoReturn([FromQuery] string partnerCode, [FromQuery] string orderId, [FromQuery] string requestId, [FromQuery] string amount, [FromQuery] string orderInfo, [FromQuery] string orderType, [FromQuery] string transId, [FromQuery] string resultCode, [FromQuery] string message, [FromQuery] string payType, [FromQuery] string responseTime, [FromQuery] string extraData, [FromQuery] string signature)
        {
            try
            {
                var isValid = _momoService.ValidateSignature(partnerCode, orderId, requestId, long.Parse(amount), orderInfo, orderType, long.Parse(transId), int.Parse(resultCode), message, payType, long.Parse(responseTime), extraData, signature);
                
                if (isValid)
                {
                    if (resultCode == "0")
                    {
                        var realOrderIdString = orderId.Split('_')[0];
                        if (int.TryParse(realOrderIdString, out int id))
                        {
                            await _orderService.UpdatePaymentStatusAsync(id, "Paid");
                        }
                        return Ok(new { message = "Thanh toán thành công qua MoMo", orderId });
                    }
                    else
                    {
                        var realOrderIdString = orderId.Split('_')[0];
                        if (int.TryParse(realOrderIdString, out int id))
                        {
                            await _orderService.UpdatePaymentStatusAsync(id, "Failed");
                        }
                        return BadRequest(new { message = "Thanh toán thất bại", orderId });
                    }
                }
                
                return BadRequest(new { message = "Invalid signature" });
            }
            catch (System.Exception ex)
            {
                return BadRequest(new { message = "Error processing request", details = ex.Message });
            }
        }

        [AllowAnonymous]
        [HttpPost("momo-notify")]
        public async Task<IActionResult> MomoNotify([FromBody] MomoIpnRequest ipnData)
        {
            try
            {
                var isValid = _momoService.ValidateSignature(
                    ipnData.partnerCode, ipnData.orderId, ipnData.requestId, ipnData.amount, 
                    ipnData.orderInfo, ipnData.orderType, ipnData.transId, ipnData.resultCode, 
                    ipnData.message, ipnData.payType, ipnData.responseTime, ipnData.extraData, 
                    ipnData.signature);

                if (isValid)
                {
                    var realOrderIdString = ipnData.orderId.Split('_')[0];
                    if (int.TryParse(realOrderIdString, out int id))
                    {
                        if (ipnData.resultCode == 0)
                        {
                            await _orderService.UpdatePaymentStatusAsync(id, "Paid");
                        }
                        else
                        {
                            await _orderService.UpdatePaymentStatusAsync(id, "Failed");
                        }
                    }
                    return NoContent();
                }

                return BadRequest(new { message = "Invalid signature" });
            }
            catch (System.Exception)
            {
                // Typically we return 204 NoContent for IPN to acknowledge receipt even if error, 
                // but we might log it.
                return NoContent();
            }
        }
    }
}

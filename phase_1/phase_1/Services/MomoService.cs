using Microsoft.Extensions.Configuration;
using phase_1.Models;
using phase_1.Services.Interfaces;
using System;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace phase_1.Services
{
    public class MomoService : IMomoService
    {
        private readonly IConfiguration _config;
        
        public MomoService(IConfiguration config)
        {
            _config = config;
        }

        public async Task<string> CreatePaymentUrl(Order order)
        {
            var partnerCode = _config["MoMo:PartnerCode"];
            var accessKey = _config["MoMo:AccessKey"];
            var secretKey = _config["MoMo:SecretKey"];
            var endpoint = _config["MoMo:Endpoint"];
            var returnUrl = _config["MoMo:ReturnUrl"];
            var notifyUrl = _config["MoMo:NotifyUrl"];
            
            var amount = order.TotalAmount.ToString("0");
            var orderId = order.Id.ToString() + "_" + DateTime.UtcNow.Ticks.ToString();
            var requestId = Guid.NewGuid().ToString();
            var orderInfo = "Thanh toan don hang " + order.Id;
            var requestType = "captureWallet";
            var extraData = "";
            
            var rawHash = $"accessKey={accessKey}&amount={amount}&extraData={extraData}&ipnUrl={notifyUrl}&orderId={orderId}&orderInfo={orderInfo}&partnerCode={partnerCode}&redirectUrl={returnUrl}&requestId={requestId}&requestType={requestType}";
            
            var signature = ComputeHmacSha256(rawHash, secretKey);
            
            var requestData = new
            {
                partnerCode = partnerCode,
                partnerName = "Test Store",
                storeId = "MomoTestStore",
                requestId = requestId,
                amount = amount,
                orderId = orderId,
                orderInfo = orderInfo,
                redirectUrl = returnUrl,
                ipnUrl = notifyUrl,
                lang = "vi",
                extraData = extraData,
                requestType = requestType,
                signature = signature
            };
            
            using var client = new HttpClient();
            var content = new StringContent(System.Text.Json.JsonSerializer.Serialize(requestData), Encoding.UTF8, "application/json");
            var response = await client.PostAsync(endpoint, content);
            var responseString = await response.Content.ReadAsStringAsync();
            
            var jsonResponse = System.Text.Json.JsonDocument.Parse(responseString);
            if (jsonResponse.RootElement.TryGetProperty("payUrl", out var payUrlElement))
            {
                return payUrlElement.GetString() ?? "";
            }
            
            return "";
        }
        
        private string ComputeHmacSha256(string message, string secretKey)
        {
            var keyBytes = Encoding.UTF8.GetBytes(secretKey!);
            var messageBytes = Encoding.UTF8.GetBytes(message);
            
            using (var hmac = new HMACSHA256(keyBytes))
            {
                var hashBytes = hmac.ComputeHash(messageBytes);
                return BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
            }
        }

        public bool ValidateSignature(string partnerCode, string orderId, string requestId, long amount, string orderInfo, string orderType, long transId, int resultCode, string message, string payType, long responseTime, string extraData, string signature)
        {
            var accessKey = _config["MoMo:AccessKey"];
            var secretKey = _config["MoMo:SecretKey"];

            var rawHash = $"accessKey={accessKey}&amount={amount}&extraData={extraData}&message={message}&orderId={orderId}&orderInfo={orderInfo}&orderType={orderType}&partnerCode={partnerCode}&payType={payType}&requestId={requestId}&responseTime={responseTime}&resultCode={resultCode}&transId={transId}";
            
            var expectedSignature = ComputeHmacSha256(rawHash, secretKey);

            return signature.Equals(expectedSignature, StringComparison.OrdinalIgnoreCase);
        }
    }
}

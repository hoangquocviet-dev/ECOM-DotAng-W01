using phase_1.Models;
using System.Threading.Tasks;

namespace phase_1.Services.Interfaces
{
    public interface IMomoService
    {
        Task<string> CreatePaymentUrl(Order order);
        bool ValidateSignature(string partnerCode, string orderId, string requestId, long amount, string orderInfo, string orderType, long transId, int resultCode, string message, string payType, long responseTime, string extraData, string signature);
    }
}

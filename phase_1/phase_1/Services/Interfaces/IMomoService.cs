using phase_1.Models;
using System.Threading.Tasks;

namespace phase_1.Services.Interfaces
{
    public interface IMomoService
    {
        Task<string> CreatePaymentUrl(Order order);
    }
}

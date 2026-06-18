using System.Collections.Generic;
using System.Threading.Tasks;
using phase_1.DTOs;

namespace phase_1.Services.Interfaces
{
    public interface IFlashSaleService
    {
        Task<IEnumerable<FlashSaleDto>> GetAllFlashSalesAsync();
        Task<FlashSaleDto?> GetFlashSaleByIdAsync(int id);
        Task<FlashSaleDto?> GetActiveFlashSaleAsync();
        Task<FlashSaleDto> CreateFlashSaleAsync(CreateFlashSaleRequest request);
        Task<bool> UpdateFlashSaleAsync(int id, UpdateFlashSaleRequest request);
        Task<bool> DeleteFlashSaleAsync(int id);
    }
}

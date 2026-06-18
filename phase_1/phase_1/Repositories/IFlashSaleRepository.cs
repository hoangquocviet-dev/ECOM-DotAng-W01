using System.Collections.Generic;
using System.Threading.Tasks;
using phase_1.Models;

namespace phase_1.Repositories
{
    public interface IFlashSaleRepository
    {
        Task<IEnumerable<FlashSale>> GetAllAsync();
        Task<FlashSale?> GetByIdAsync(int id);
        Task<FlashSale?> GetActiveFlashSaleAsync();
        Task<FlashSale> CreateAsync(FlashSale flashSale);
        Task UpdateAsync(FlashSale flashSale);
        Task DeleteAsync(int id);
    }
}

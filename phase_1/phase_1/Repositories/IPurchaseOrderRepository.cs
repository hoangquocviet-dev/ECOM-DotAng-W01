using phase_1.DTOs;
using phase_1.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace phase_1.Repositories
{
    public interface IPurchaseOrderRepository
    {
        Task<IEnumerable<PurchaseOrderDto>> GetAllAsync();
        Task<PurchaseOrderDto?> GetByIdAsync(int id);
        Task<PurchaseOrder> CreateAsync(PurchaseOrder po, IEnumerable<PurchaseOrderDetail> details);
        Task<bool> CompleteOrderAsync(int id);
    }
}

using System.Collections.Generic;
using System.Threading.Tasks;
using phase_1.DTOs;
using phase_1.Models;

namespace phase_1.Repositories
{
    public interface ISupplierRepository
    {
        Task<IEnumerable<SupplierDto>> GetAllAsync();
        Task<SupplierDto?> GetByIdAsync(int id);
        Task<Supplier> CreateAsync(Supplier supplier);
        Task UpdateAsync(Supplier supplier);
        Task DeleteAsync(int id);
    }
}

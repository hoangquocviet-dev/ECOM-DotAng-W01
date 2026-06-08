using phase_1.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace phase_1.Repositories
{
    public interface IReturnRequestRepository
    {
        Task<IEnumerable<ReturnRequest>> GetAllAsync();
        Task<IEnumerable<ReturnRequest>> GetByUserIdAsync(int userId);
        Task<ReturnRequest?> GetByIdAsync(int id);
        Task<ReturnRequest> AddAsync(ReturnRequest request);
        Task<ReturnRequest> UpdateAsync(ReturnRequest request);
    }
}

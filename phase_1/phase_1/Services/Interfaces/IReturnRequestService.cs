using phase_1.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace phase_1.Services.Interfaces
{
    public interface IReturnRequestService
    {
        Task<IEnumerable<ReturnRequestDto>> GetAllAsync();
        Task<IEnumerable<ReturnRequestDto>> GetByUserIdAsync(int userId);
        Task<ReturnRequestDto?> GetByIdAsync(int id);
        Task<ReturnRequestDto?> CreateRequestAsync(int userId, CreateReturnRequest request);
        Task<ReturnRequestDto?> ProcessRequestAsync(int id, ProcessReturnRequest request);
    }
}

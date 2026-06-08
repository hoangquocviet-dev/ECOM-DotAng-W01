using phase_1.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace phase_1.Services.Interfaces
{
    public interface IComboService
    {
        Task<IEnumerable<ComboDto>> GetAllCombosAsync(bool includeInactive = false);
        Task<ComboDto?> GetComboByIdAsync(int id);
        Task<ComboDto> CreateComboAsync(CreateComboRequest request);
        Task<ComboDto?> UpdateComboAsync(int id, UpdateComboRequest request);
        Task<bool> DeleteComboAsync(int id);
    }
}

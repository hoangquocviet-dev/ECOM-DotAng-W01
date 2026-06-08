using phase_1.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace phase_1.Repositories
{
    public interface IComboRepository
    {
        Task<IEnumerable<Combo>> GetAllAsync();
        Task<IEnumerable<Combo>> GetAllActiveAsync();
        Task<Combo?> GetByIdAsync(int id);
        Task<Combo> AddAsync(Combo combo);
        Task<Combo> UpdateAsync(Combo combo);
        Task DeleteAsync(Combo combo);
    }
}

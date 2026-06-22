using phase_1.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace phase_1.Repositories
{
    public interface IPageRepository
    {
        Task<IEnumerable<Page>> GetAllAsync(bool includeInactive = false);
        Task<Page?> GetByIdAsync(int id);
        Task<Page?> GetBySlugAsync(string slug);
        Task AddAsync(Page page);
        Task UpdateAsync(Page page);
        Task DeleteAsync(Page page);
    }
}

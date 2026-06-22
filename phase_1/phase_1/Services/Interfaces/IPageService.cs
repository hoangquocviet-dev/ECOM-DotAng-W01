using phase_1.DTOs;
using phase_1.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace phase_1.Services.Interfaces
{
    public interface IPageService
    {
        Task<IEnumerable<Page>> GetAllPagesAsync(bool includeInactive = false);
        Task<Page?> GetPageByIdAsync(int id);
        Task<Page?> GetPageBySlugAsync(string slug);
        Task<Page> CreatePageAsync(CreatePageRequest request);
        Task<Page?> UpdatePageAsync(int id, UpdatePageRequest request);
        Task<Page?> DeletePageAsync(int id);
    }
}

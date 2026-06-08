using phase_1.DTOs;

namespace phase_1.Services.Interfaces
{
    public interface IBlogService
    {
        Task<IEnumerable<BlogDto>> GetAllBlogsAsync();
        Task<BlogDto?> GetBlogByIdAsync(int id);
        Task<BlogDto> AddBlogAsync(AddBlogRequest request);
        Task<BlogDto?> UpdateBlogAsync(int id, UpdateBlogRequest request);
        Task<bool> DeleteBlogAsync(int id);
    }
}

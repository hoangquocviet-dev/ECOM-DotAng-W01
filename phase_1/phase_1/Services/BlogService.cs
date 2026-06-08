using phase_1.DTOs;
using phase_1.Models;
using phase_1.Repositories;
using phase_1.Services.Interfaces;

namespace phase_1.Services
{
    public class BlogService : IBlogService
    {
        private readonly IBlogRepository _blogRepository;

        public BlogService(IBlogRepository blogRepository)
        {
            _blogRepository = blogRepository;
        }

        public async Task<IEnumerable<BlogDto>> GetAllBlogsAsync()
        {
            var blogs = await _blogRepository.GetAllAsync();
            return blogs.Select(b => new BlogDto
            {
                Id = b.Id,
                Title = b.Title,
                Content = b.Content,
                ImageUrl = b.ImageUrl,
                Author = b.Author,
                CreatedAt = b.CreatedAt,
                UpdatedAt = b.UpdatedAt
            });
        }

        public async Task<BlogDto?> GetBlogByIdAsync(int id)
        {
            var blog = await _blogRepository.GetByIdAsync(id);
            if (blog == null) return null;

            return new BlogDto
            {
                Id = blog.Id,
                Title = blog.Title,
                Content = blog.Content,
                ImageUrl = blog.ImageUrl,
                Author = blog.Author,
                CreatedAt = blog.CreatedAt,
                UpdatedAt = blog.UpdatedAt
            };
        }

        public async Task<BlogDto> AddBlogAsync(AddBlogRequest request)
        {
            var blog = new Blog
            {
                Title = request.Title,
                Content = request.Content,
                ImageUrl = request.ImageUrl,
                Author = request.Author,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            var addedBlog = await _blogRepository.AddAsync(blog);

            return new BlogDto
            {
                Id = addedBlog.Id,
                Title = addedBlog.Title,
                Content = addedBlog.Content,
                ImageUrl = addedBlog.ImageUrl,
                Author = addedBlog.Author,
                CreatedAt = addedBlog.CreatedAt,
                UpdatedAt = addedBlog.UpdatedAt
            };
        }

        public async Task<BlogDto?> UpdateBlogAsync(int id, UpdateBlogRequest request)
        {
            var blog = await _blogRepository.GetByIdAsync(id);
            if (blog == null) return null;

            blog.Title = request.Title;
            blog.Content = request.Content;
            blog.ImageUrl = request.ImageUrl;
            blog.Author = request.Author;
            blog.UpdatedAt = DateTime.UtcNow;

            var updatedBlog = await _blogRepository.UpdateAsync(blog);

            return new BlogDto
            {
                Id = updatedBlog.Id,
                Title = updatedBlog.Title,
                Content = updatedBlog.Content,
                ImageUrl = updatedBlog.ImageUrl,
                Author = updatedBlog.Author,
                CreatedAt = updatedBlog.CreatedAt,
                UpdatedAt = updatedBlog.UpdatedAt
            };
        }

        public async Task<bool> DeleteBlogAsync(int id)
        {
            var blog = await _blogRepository.GetByIdAsync(id);
            if (blog == null) return false;

            await _blogRepository.DeleteAsync(blog);
            return true;
        }
    }
}

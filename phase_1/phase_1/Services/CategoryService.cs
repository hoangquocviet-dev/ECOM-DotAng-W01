using phase_1.Models;
using phase_1.Repositories;
using phase_1.Services.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace phase_1.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;

        public CategoryService(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public async Task<IEnumerable<Category>> GetAllCategoriesAsync()
        {
            return await _categoryRepository.GetAllAsync();
        }

        public async Task<Category?> GetCategoryByIdAsync(int id)
        {
            return await _categoryRepository.GetByIdAsync(id);
        }

        public async Task<Category?> GetCategoryBySlugAsync(string slug)
        {
            return await _categoryRepository.GetBySlugAsync(slug);
        }

        public async Task<Category> CreateCategoryAsync(Category category)
        {
            if (string.IsNullOrEmpty(category.Slug))
            {
                category.Slug = phase_1.Helpers.StringHelper.GenerateSlug(category.Name);
            }
            await _categoryRepository.AddAsync(category);
            return category;
        }

        public async Task<Category> UpdateCategoryAsync(Category category)
        {
            if (string.IsNullOrEmpty(category.Slug))
            {
                category.Slug = phase_1.Helpers.StringHelper.GenerateSlug(category.Name);
            }
            await _categoryRepository.UpdateAsync(category);
            return category;
        }

        public async Task<Category?> DeleteCategoryAsync(int id)
        {
            var category = await _categoryRepository.GetByIdAsync(id);
            if (category != null)
            {
                await _categoryRepository.DeleteAsync(category);
            }
            return category;
        }
    }
}

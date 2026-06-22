using phase_1.DTOs;
using phase_1.Helpers;
using phase_1.Models;
using phase_1.Repositories;
using phase_1.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace phase_1.Services
{
    public class PageService : IPageService
    {
        private readonly IPageRepository _pageRepository;

        public PageService(IPageRepository pageRepository)
        {
            _pageRepository = pageRepository;
        }

        public async Task<IEnumerable<Page>> GetAllPagesAsync(bool includeInactive = false)
        {
            return await _pageRepository.GetAllAsync(includeInactive);
        }

        public async Task<Page?> GetPageByIdAsync(int id)
        {
            return await _pageRepository.GetByIdAsync(id);
        }

        public async Task<Page?> GetPageBySlugAsync(string slug)
        {
            return await _pageRepository.GetBySlugAsync(slug);
        }

        public async Task<Page> CreatePageAsync(CreatePageRequest request)
        {
            string slug = string.IsNullOrEmpty(request.Slug) 
                ? StringHelper.GenerateSlug(request.Title) 
                : request.Slug;

            var page = new Page
            {
                Title = request.Title,
                Slug = slug,
                Content = request.Content,
                IsActive = request.IsActive,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            await _pageRepository.AddAsync(page);
            return page;
        }

        public async Task<Page?> UpdatePageAsync(int id, UpdatePageRequest request)
        {
            var page = await _pageRepository.GetByIdAsync(id);
            if (page == null) return null;

            string slug = string.IsNullOrEmpty(request.Slug) 
                ? StringHelper.GenerateSlug(request.Title) 
                : request.Slug;

            page.Title = request.Title;
            page.Slug = slug;
            page.Content = request.Content;
            page.IsActive = request.IsActive;
            page.UpdatedAt = DateTime.UtcNow;

            await _pageRepository.UpdateAsync(page);
            return page;
        }

        public async Task<Page?> DeletePageAsync(int id)
        {
            var page = await _pageRepository.GetByIdAsync(id);
            if (page != null)
            {
                await _pageRepository.DeleteAsync(page);
            }
            return page;
        }
    }
}

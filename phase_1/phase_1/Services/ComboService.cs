using phase_1.DTOs;
using phase_1.Models;
using phase_1.Repositories;
using phase_1.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace phase_1.Services
{
    public class ComboService : IComboService
    {
        private readonly IComboRepository _comboRepository;

        public ComboService(IComboRepository comboRepository)
        {
            _comboRepository = comboRepository;
        }

        public async Task<IEnumerable<ComboDto>> GetAllCombosAsync(bool includeInactive = false)
        {
            var combos = includeInactive ? await _comboRepository.GetAllAsync() : await _comboRepository.GetAllActiveAsync();
            return combos.Select(MapToDto);
        }

        public async Task<ComboDto?> GetComboByIdAsync(int id)
        {
            var combo = await _comboRepository.GetByIdAsync(id);
            if (combo == null) return null;
            return MapToDto(combo);
        }

        public async Task<ComboDto> CreateComboAsync(CreateComboRequest request)
        {
            var combo = new Combo
            {
                Name = request.Name,
                Description = request.Description,
                Price = request.Price,
                ImageUrl = request.ImageUrl,
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            };

            foreach (var item in request.Items)
            {
                combo.ComboItems.Add(new ComboItem
                {
                    ProductId = item.ProductId,
                    Quantity = item.Quantity
                });
            }

            var createdCombo = await _comboRepository.AddAsync(combo);
            var loadedCombo = await _comboRepository.GetByIdAsync(createdCombo.Id); 
            return MapToDto(loadedCombo!);
        }

        public async Task<ComboDto?> UpdateComboAsync(int id, UpdateComboRequest request)
        {
            var combo = await _comboRepository.GetByIdAsync(id);
            if (combo == null) return null;

            combo.Name = request.Name;
            combo.Description = request.Description;
            combo.Price = request.Price;
            combo.ImageUrl = request.ImageUrl;
            combo.IsActive = request.IsActive;

            var updatedCombo = await _comboRepository.UpdateAsync(combo);
            return MapToDto(updatedCombo);
        }

        public async Task<bool> DeleteComboAsync(int id)
        {
            var combo = await _comboRepository.GetByIdAsync(id);
            if (combo == null) return false;

            await _comboRepository.DeleteAsync(combo);
            return true;
        }

        private ComboDto MapToDto(Combo combo)
        {
            return new ComboDto
            {
                Id = combo.Id,
                Name = combo.Name,
                Description = combo.Description,
                Price = combo.Price,
                ImageUrl = combo.ImageUrl,
                IsActive = combo.IsActive,
                CreatedAt = combo.CreatedAt,
                Items = combo.ComboItems.Select(ci => new ComboItemDto
                {
                    ProductId = ci.ProductId,
                    ProductName = ci.Product?.Name ?? "Unknown",
                    Quantity = ci.Quantity,
                    OriginalPrice = ci.Product?.Price ?? 0
                }).ToList()
            };
        }
    }
}

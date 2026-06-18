using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using phase_1.DTOs;
using phase_1.Models;
using phase_1.Repositories;
using phase_1.Services.Interfaces;

namespace phase_1.Services
{
    public class FlashSaleService : IFlashSaleService
    {
        private readonly IFlashSaleRepository _flashSaleRepository;

        public FlashSaleService(IFlashSaleRepository flashSaleRepository)
        {
            _flashSaleRepository = flashSaleRepository;
        }

        public async Task<IEnumerable<FlashSaleDto>> GetAllFlashSalesAsync()
        {
            var flashSales = await _flashSaleRepository.GetAllAsync();
            return flashSales.Select(MapToDto);
        }

        public async Task<FlashSaleDto?> GetFlashSaleByIdAsync(int id)
        {
            var flashSale = await _flashSaleRepository.GetByIdAsync(id);
            if (flashSale == null) return null;
            return MapToDto(flashSale);
        }

        public async Task<FlashSaleDto?> GetActiveFlashSaleAsync()
        {
            var activeSale = await _flashSaleRepository.GetActiveFlashSaleAsync();
            if (activeSale == null) return null;
            return MapToDto(activeSale);
        }

        public async Task<FlashSaleDto> CreateFlashSaleAsync(CreateFlashSaleRequest request)
        {
            var flashSale = new FlashSale
            {
                Name = request.Name,
                Description = request.Description,
                StartTime = request.StartTime,
                EndTime = request.EndTime,
                IsActive = request.IsActive,
                FlashSaleItems = request.Items.Select(i => new FlashSaleItem
                {
                    ProductId = i.ProductId,
                    DiscountPrice = i.DiscountPrice,
                    Quantity = i.Quantity,
                    MaxPerCustomer = i.MaxPerCustomer,
                    SoldQuantity = 0
                }).ToList()
            };

            var created = await _flashSaleRepository.CreateAsync(flashSale);
            var result = await _flashSaleRepository.GetByIdAsync(created.Id);
            return MapToDto(result!);
        }

        public async Task<bool> UpdateFlashSaleAsync(int id, UpdateFlashSaleRequest request)
        {
            var flashSale = await _flashSaleRepository.GetByIdAsync(id);
            if (flashSale == null) return false;

            flashSale.Name = request.Name;
            flashSale.Description = request.Description;
            flashSale.StartTime = request.StartTime;
            flashSale.EndTime = request.EndTime;
            flashSale.IsActive = request.IsActive;

            var requestItemIds = request.Items.Where(i => i.Id.HasValue).Select(i => i.Id!.Value).ToList();
            var itemsToRemove = flashSale.FlashSaleItems.Where(i => !requestItemIds.Contains(i.Id)).ToList();
            foreach (var item in itemsToRemove)
            {
                flashSale.FlashSaleItems.Remove(item);
            }
            foreach (var reqItem in request.Items)
            {
                if (reqItem.Id.HasValue)
                {
                    var existingItem = flashSale.FlashSaleItems.FirstOrDefault(i => i.Id == reqItem.Id.Value);
                    if (existingItem != null)
                    {
                        existingItem.ProductId = reqItem.ProductId;
                        existingItem.DiscountPrice = reqItem.DiscountPrice;
                        existingItem.Quantity = reqItem.Quantity;
                        existingItem.MaxPerCustomer = reqItem.MaxPerCustomer;
                    }
                }
                else
                {
                    flashSale.FlashSaleItems.Add(new FlashSaleItem
                    {
                        ProductId = reqItem.ProductId,
                        DiscountPrice = reqItem.DiscountPrice,
                        Quantity = reqItem.Quantity,
                        MaxPerCustomer = reqItem.MaxPerCustomer,
                        SoldQuantity = 0
                    });
                }
            }

            await _flashSaleRepository.UpdateAsync(flashSale);
            return true;
        }

        public async Task<bool> DeleteFlashSaleAsync(int id)
        {
            var flashSale = await _flashSaleRepository.GetByIdAsync(id);
            if (flashSale == null) return false;
            await _flashSaleRepository.DeleteAsync(id);
            return true;
        }

        private static FlashSaleDto MapToDto(FlashSale flashSale)
        {
            return new FlashSaleDto
            {
                Id = flashSale.Id,
                Name = flashSale.Name,
                Description = flashSale.Description,
                StartTime = flashSale.StartTime,
                EndTime = flashSale.EndTime,
                IsActive = flashSale.IsActive,
                FlashSaleItems = flashSale.FlashSaleItems.Select(i => new FlashSaleItemDto
                {
                    Id = i.Id,
                    ProductId = i.ProductId,
                    ProductName = i.Product?.Name ?? string.Empty,
                    ProductImage = i.Product?.ImageUrl ?? string.Empty,
                    OriginalPrice = i.Product?.Price ?? 0,
                    DiscountPrice = i.DiscountPrice,
                    Quantity = i.Quantity,
                    SoldQuantity = i.SoldQuantity,
                    MaxPerCustomer = i.MaxPerCustomer
                }).ToList()
            };
        }
    }
}

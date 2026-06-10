using phase_1.DTOs;
using phase_1.Models;
using phase_1.Repositories;
using phase_1.Services.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace phase_1.Services
{
    public class PurchaseOrderService : IPurchaseOrderService
    {
        private readonly IPurchaseOrderRepository _purchaseOrderRepository;

        public PurchaseOrderService(IPurchaseOrderRepository purchaseOrderRepository)
        {
            _purchaseOrderRepository = purchaseOrderRepository;
        }

        public async Task<IEnumerable<PurchaseOrderDto>> GetAllAsync()
        {
            return await _purchaseOrderRepository.GetAllAsync();
        }

        public async Task<PurchaseOrderDto?> GetByIdAsync(int id)
        {
            return await _purchaseOrderRepository.GetByIdAsync(id);
        }

        public async Task<PurchaseOrderDto> CreateAsync(CreatePurchaseOrderDto createDto)
        {
            var po = new PurchaseOrder
            {
                SupplierId = createDto.SupplierId,
                Notes = createDto.Notes,
                TotalAmount = createDto.Items.Sum(i => i.Quantity * i.UnitPrice),
                Status = "Pending"
            };

            var details = createDto.Items.Select(i => new PurchaseOrderDetail
            {
                ProductId = i.ProductId,
                Quantity = i.Quantity,
                UnitPrice = i.UnitPrice
            });

            var createdPo = await _purchaseOrderRepository.CreateAsync(po, details);
            return await GetByIdAsync(createdPo.Id) ?? new PurchaseOrderDto();
        }

        public async Task<bool> CompleteOrderAsync(int id)
        {
            return await _purchaseOrderRepository.CompleteOrderAsync(id);
        }
    }
}

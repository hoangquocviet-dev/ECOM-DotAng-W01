using Microsoft.EntityFrameworkCore;
using phase_1.Data;
using phase_1.DTOs;
using phase_1.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace phase_1.Repositories
{
    public class PurchaseOrderRepository : IPurchaseOrderRepository
    {
        private readonly ApplicationDbContext _context;

        public PurchaseOrderRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<PurchaseOrderDto>> GetAllAsync()
        {
            return await _context.PurchaseOrders
                .Include(p => p.Supplier)
                .Select(p => new PurchaseOrderDto
                {
                    Id = p.Id,
                    SupplierId = p.SupplierId,
                    SupplierName = p.Supplier.Name,
                    OrderDate = p.OrderDate,
                    TotalAmount = p.TotalAmount,
                    Status = p.Status,
                    Notes = p.Notes
                })
                .OrderByDescending(p => p.OrderDate)
                .ToListAsync();
        }

        public async Task<PurchaseOrderDto?> GetByIdAsync(int id)
        {
            var po = await _context.PurchaseOrders
                .Include(p => p.Supplier)
                .Include(p => p.PurchaseOrderDetails)
                    .ThenInclude(d => d.Product)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (po == null) return null;

            return new PurchaseOrderDto
            {
                Id = po.Id,
                SupplierId = po.SupplierId,
                SupplierName = po.Supplier.Name,
                OrderDate = po.OrderDate,
                TotalAmount = po.TotalAmount,
                Status = po.Status,
                Notes = po.Notes,
                Items = po.PurchaseOrderDetails.Select(d => new PurchaseOrderDetailDto
                {
                    ProductId = d.ProductId,
                    ProductName = d.Product.Name,
                    Quantity = d.Quantity,
                    UnitPrice = d.UnitPrice
                })
            };
        }

        public async Task<PurchaseOrder> CreateAsync(PurchaseOrder po, IEnumerable<PurchaseOrderDetail> details)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                _context.PurchaseOrders.Add(po);
                await _context.SaveChangesAsync();

                foreach (var detail in details)
                {
                    detail.PurchaseOrderId = po.Id;
                    _context.PurchaseOrderDetails.Add(detail);
                }

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return po;
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        public async Task<bool> CompleteOrderAsync(int id)
        {
            var po = await _context.PurchaseOrders
                .Include(p => p.PurchaseOrderDetails)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (po == null || po.Status != "Pending") return false;

            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                po.Status = "Completed";

                foreach (var detail in po.PurchaseOrderDetails)
                {
                    var product = await _context.Products.FindAsync(detail.ProductId);
                    if (product != null)
                    {
                        product.StockQuantity += detail.Quantity;
                        _context.Products.Update(product);
                    }
                }

                _context.PurchaseOrders.Update(po);
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
                return true;
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }
    }
}

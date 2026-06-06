using Microsoft.EntityFrameworkCore;
using phase_1.Data;
using phase_1.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace phase_1.Repositories
{
    public class VoucherRepository : IVoucherRepository
    {
        private readonly ApplicationDbContext _context;

        public VoucherRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Voucher>> GetAllVouchersAsync()
        {
            return await _context.Vouchers.ToListAsync();
        }

        public async Task<Voucher?> GetVoucherByIdAsync(int id)
        {
            return await _context.Vouchers.FindAsync(id);
        }

        public async Task<Voucher?> GetVoucherByCodeAsync(string code)
        {
            return await _context.Vouchers.FirstOrDefaultAsync(v => v.Code == code);
        }

        public async Task AddVoucherAsync(Voucher voucher)
        {
            _context.Vouchers.Add(voucher);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateVoucherAsync(Voucher voucher)
        {
            _context.Vouchers.Update(voucher);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteVoucherAsync(Voucher voucher)
        {
            _context.Vouchers.Remove(voucher);
            await _context.SaveChangesAsync();
        }
    }
}

using phase_1.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace phase_1.Repositories
{
    public interface IVoucherRepository
    {
        Task<IEnumerable<Voucher>> GetAllVouchersAsync();
        Task<Voucher?> GetVoucherByIdAsync(int id);
        Task<Voucher?> GetVoucherByCodeAsync(string code);
        Task AddVoucherAsync(Voucher voucher);
        Task UpdateVoucherAsync(Voucher voucher);
        Task DeleteVoucherAsync(Voucher voucher);
    }
}

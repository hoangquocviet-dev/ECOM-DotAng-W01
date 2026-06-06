using phase_1.DTOs;
using phase_1.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace phase_1.Services.Interfaces
{
    public interface IVoucherService
    {
        Task<IEnumerable<Voucher>> GetAllVouchersAsync();
        Task<Voucher?> GetVoucherByIdAsync(int id);
        Task<Voucher?> GetVoucherByCodeAsync(string code);
        Task<Voucher> CreateVoucherAsync(CreateVoucherRequest request);
        Task<Voucher?> UpdateVoucherAsync(int id, UpdateVoucherRequest request);
        Task<bool> DeleteVoucherAsync(int id);
    }
}

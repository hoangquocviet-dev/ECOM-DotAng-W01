using phase_1.DTOs;
using phase_1.Models;
using phase_1.Repositories;
using phase_1.Services.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;
using phase_1.Hubs;
using Microsoft.AspNetCore.SignalR;

namespace phase_1.Services
{
    public class VoucherService : IVoucherService
    {
        private readonly IVoucherRepository _voucherRepository;
        private readonly IHubContext<NotificationHub> _hubContext;

        public VoucherService(IVoucherRepository voucherRepository, IHubContext<NotificationHub> hubContext)
        {
            _voucherRepository = voucherRepository;
            _hubContext = hubContext;
        }

        public async Task<IEnumerable<Voucher>> GetAllVouchersAsync()
        {
            return await _voucherRepository.GetAllVouchersAsync();
        }

        public async Task<Voucher?> GetVoucherByIdAsync(int id)
        {
            return await _voucherRepository.GetVoucherByIdAsync(id);
        }

        public async Task<Voucher?> GetVoucherByCodeAsync(string code)
        {
            return await _voucherRepository.GetVoucherByCodeAsync(code);
        }

        public async Task<Voucher> CreateVoucherAsync(CreateVoucherRequest request)
        {
            var voucher = new Voucher
            {
                Code = request.Code.ToUpper(),
                DiscountAmount = request.DiscountAmount,
                ExpiryDate = request.ExpiryDate,
                UsageLimit = request.UsageLimit,
                UsedCount = 0
            };

            await _voucherRepository.AddVoucherAsync(voucher);

            await _hubContext.Clients.All.SendAsync("ReceiveNewPromotion", voucher.Code, voucher.DiscountAmount);

            return voucher;
        }

        public async Task<Voucher?> UpdateVoucherAsync(int id, UpdateVoucherRequest request)
        {
            var voucher = await _voucherRepository.GetVoucherByIdAsync(id);
            if (voucher == null) return null;

            voucher.DiscountAmount = request.DiscountAmount;
            voucher.ExpiryDate = request.ExpiryDate;
            voucher.UsageLimit = request.UsageLimit;

            await _voucherRepository.UpdateVoucherAsync(voucher);
            return voucher;
        }

        public async Task<bool> DeleteVoucherAsync(int id)
        {
            var voucher = await _voucherRepository.GetVoucherByIdAsync(id);
            if (voucher == null) return false;

            await _voucherRepository.DeleteVoucherAsync(voucher);
            return true;
        }
    }
}

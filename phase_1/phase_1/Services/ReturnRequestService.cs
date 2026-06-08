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
    public class ReturnRequestService : IReturnRequestService
    {
        private readonly IReturnRequestRepository _returnRepository;
        private readonly IOrderRepository _orderRepository;

        public ReturnRequestService(IReturnRequestRepository returnRepository, IOrderRepository orderRepository)
        {
            _returnRepository = returnRepository;
            _orderRepository = orderRepository;
        }

        public async Task<IEnumerable<ReturnRequestDto>> GetAllAsync()
        {
            var requests = await _returnRepository.GetAllAsync();
            return requests.Select(MapToDto);
        }

        public async Task<IEnumerable<ReturnRequestDto>> GetByUserIdAsync(int userId)
        {
            var requests = await _returnRepository.GetByUserIdAsync(userId);
            return requests.Select(MapToDto);
        }

        public async Task<ReturnRequestDto?> GetByIdAsync(int id)
        {
            var request = await _returnRepository.GetByIdAsync(id);
            return request == null ? null : MapToDto(request);
        }

        public async Task<ReturnRequestDto?> CreateRequestAsync(int userId, CreateReturnRequest request)
        {
            var order = await _orderRepository.GetOrderByIdAsync(request.OrderId);
            if (order == null || order.UserId != userId)
            {
                throw new ArgumentException("Đơn hàng không tồn tại hoặc không thuộc về người dùng này.");
            }

            if (order.Status != "Completed")
            {
                throw new ArgumentException("Chỉ có thể yêu cầu hoàn hàng cho các đơn hàng đã hoàn thành.");
            }

            var newRequest = new ReturnRequest
            {
                OrderId = request.OrderId,
                UserId = userId,
                Reason = request.Reason,
                Status = "Pending",
                RequestDate = DateTime.UtcNow
            };

            var savedRequest = await _returnRepository.AddAsync(newRequest);
            return MapToDto(savedRequest);
        }

        public async Task<ReturnRequestDto?> ProcessRequestAsync(int id, ProcessReturnRequest request)
        {
            var returnRequest = await _returnRepository.GetByIdAsync(id);
            if (returnRequest == null) return null;

            returnRequest.Status = request.Status;
            returnRequest.AdminNote = request.AdminNote;
            returnRequest.ProcessedDate = DateTime.UtcNow;


            if (request.Status == "Approved")
            {
                var order = await _orderRepository.GetOrderByIdAsync(returnRequest.OrderId);
                if (order != null)
                {
                    order.Status = "Returned";
                    await _orderRepository.UpdateOrderAsync(order);
                }
            }

            var updatedRequest = await _returnRepository.UpdateAsync(returnRequest);
            return MapToDto(updatedRequest);
        }

        private ReturnRequestDto MapToDto(ReturnRequest r)
        {
            return new ReturnRequestDto
            {
                Id = r.Id,
                OrderId = r.OrderId,
                UserId = r.UserId,
                UserName = r.User?.Name ?? "N/A",
                Reason = r.Reason,
                Status = r.Status,
                RequestDate = r.RequestDate,
                ProcessedDate = r.ProcessedDate,
                AdminNote = r.AdminNote
            };
        }
    }
}

using Microsoft.EntityFrameworkCore;
using phase_1.Data;
using phase_1.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace phase_1.Repositories
{
    public class ReturnRequestRepository : IReturnRequestRepository
    {
        private readonly ApplicationDbContext _context;

        public ReturnRequestRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ReturnRequest>> GetAllAsync()
        {
            return await _context.ReturnRequests
                .Include(r => r.User)
                .Include(r => r.Order)
                .OrderByDescending(r => r.RequestDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<ReturnRequest>> GetByUserIdAsync(int userId)
        {
            return await _context.ReturnRequests
                .Include(r => r.Order)
                .Where(r => r.UserId == userId)
                .OrderByDescending(r => r.RequestDate)
                .ToListAsync();
        }

        public async Task<ReturnRequest?> GetByIdAsync(int id)
        {
            return await _context.ReturnRequests
                .Include(r => r.User)
                .Include(r => r.Order)
                .FirstOrDefaultAsync(r => r.Id == id);
        }

        public async Task<ReturnRequest> AddAsync(ReturnRequest request)
        {
            _context.ReturnRequests.Add(request);
            await _context.SaveChangesAsync();
            return request;
        }

        public async Task<ReturnRequest> UpdateAsync(ReturnRequest request)
        {
            _context.ReturnRequests.Update(request);
            await _context.SaveChangesAsync();
            return request;
        }
    }
}

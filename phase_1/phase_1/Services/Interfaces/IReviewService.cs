using phase_1.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace phase_1.Services.Interfaces
{
    public interface IReviewService
    {
        Task<IEnumerable<ReviewDto>> GetProductReviewsAsync(int productId);
        Task<ReviewDto?> AddReviewAsync(int userId, AddReviewRequest request);
        Task<ReviewDto?> ReplyReviewAsync(int reviewId, ReplyReviewRequest request);
    }
}

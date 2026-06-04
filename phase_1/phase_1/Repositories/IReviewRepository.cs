using phase_1.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace phase_1.Repositories
{
    public interface IReviewRepository
    {
        Task<IEnumerable<Review>> GetReviewsByProductIdAsync(int productId);
        Task<Review?> GetReviewByUserAndProductAsync(int userId, int productId);
        Task AddReviewAsync(Review review);
    }
}

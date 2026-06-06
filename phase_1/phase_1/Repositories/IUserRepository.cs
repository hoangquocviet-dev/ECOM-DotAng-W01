using phase_1.Models;
using System.Threading.Tasks;

namespace phase_1.Repositories
{
    public interface IUserRepository
    {
        Task<Users?> GetUserByIdAsync(int userId);
        Task<Users?> GetUserByEmailAsync(string email);
        Task UpdateUserAsync(Users user);
    }
}

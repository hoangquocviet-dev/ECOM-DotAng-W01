using phase_1.Models;
using phase_1.DTOs;
using System.Threading.Tasks;

namespace phase_1.Services.Interfaces
{
    public interface IUserService
    {
        Task<Users?> GetUserProfileAsync(int userId);
        Task<Users?> UpdateUserProfileAsync(int userId, UpdateProfileRequest request);
        Task<bool> ChangePasswordAsync(int userId, ChangePasswordRequest request);
        Task<bool> ForgotPasswordAsync(ForgotPasswordRequest request);
        Task<bool> ResetPasswordAsync(ResetPasswordRequest request);
    }
}

using phase_1.Models;
using phase_1.DTOs;
using phase_1.Repositories;
using phase_1.Services.Interfaces;
using System.Threading.Tasks;

namespace phase_1.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<Users?> GetUserProfileAsync(int userId)
        {
            return await _userRepository.GetUserByIdAsync(userId);
        }

        public async Task<Users?> UpdateUserProfileAsync(int userId, UpdateProfileRequest request)
        {
            var user = await _userRepository.GetUserByIdAsync(userId);
            if (user == null) return null;

            user.Name = request.Name;
            user.PhoneNumber = request.PhoneNumber;
            user.Address = request.Address;

            await _userRepository.UpdateUserAsync(user);
            return user;
        }
    }
}

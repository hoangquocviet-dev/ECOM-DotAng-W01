using phase_1.Models;
using phase_1.DTOs;
using phase_1.Repositories;
using phase_1.Services.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;

namespace phase_1.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly EmailService _emailService;

        public UserService(IUserRepository userRepository, EmailService emailService)
        {
            _userRepository = userRepository;
            _emailService = emailService;
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

        public async Task<bool> ChangePasswordAsync(int userId, ChangePasswordRequest request)
        {
            var user = await _userRepository.GetUserByIdAsync(userId);
            if (user == null) return false;

            if (!BCrypt.Net.BCrypt.Verify(request.OldPassword, user.PasswordHash))
            {
                return false;
            }

            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.NewPassword);
            await _userRepository.UpdateUserAsync(user);
            return true;
        }

        public async Task<bool> ForgotPasswordAsync(ForgotPasswordRequest request)
        {
            var user = await _userRepository.GetUserByEmailAsync(request.Email);
            if (user == null) return false;

            Random random = new Random();
            string otp = random.Next(100000, 999999).ToString();
            
            user.OtpCode = otp;
            await _userRepository.UpdateUserAsync(user);

            _emailService.SendPasswordResetEmail(user.Email, otp);
            return true;
        }

        public async Task<bool> ResetPasswordAsync(ResetPasswordRequest request)
        {
            var user = await _userRepository.GetUserByEmailAsync(request.Email);
            if (user == null || string.IsNullOrEmpty(user.OtpCode) || user.OtpCode != request.OtpCode) return false;

            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.NewPassword);
            user.OtpCode = string.Empty; // clear otp after success
            
            await _userRepository.UpdateUserAsync(user);
            return true;
        }

        public async Task<IEnumerable<Users>> GetAllUsersAsync()
        {
            return await _userRepository.GetAllUsersAsync();
        }

        public async Task<bool> LockUnlockUserAsync(int userId, bool isLocked)
        {
            var user = await _userRepository.GetUserByIdAsync(userId);
            if (user == null) return false;

            user.IsLocked = isLocked;
            await _userRepository.UpdateUserAsync(user);
            return true;
        }

        public async Task<bool> ChangeUserRoleAsync(int userId, string newRole)
        {
            var user = await _userRepository.GetUserByIdAsync(userId);
            if (user == null) return false;

            user.Role = newRole;
            await _userRepository.UpdateUserAsync(user);
            return true;
        }
    }
}

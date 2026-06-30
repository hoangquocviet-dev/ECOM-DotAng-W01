namespace phase_1.Models
{
    public class Users
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
        public string PasswordHash { get; set; }
        public string PhoneNumber { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string Role { get; set; } = "User";
        public bool IsEmailVerified { get; set; } = false;
        public bool IsLocked { get; set; } = false;
        public string OtpCode { get; set; }
        public string? ResetPasswordToken { get; set; }
        public DateTime? ResetPasswordTokenExpiry { get; set; }
        public decimal TotalSpent { get; set; } = 0;
        public int RewardPoints { get; set; } = 0;
        public string MemberTier { get; set; } = "Bronze";
        public string ReferralCode { get; set; } = string.Empty;
        public int? ReferredById { get; set; }
    }
}

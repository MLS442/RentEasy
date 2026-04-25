namespace RentEasyAPI.Models
{
    public class User
    {
        public int UserId { get; set; }
        public string Role { get; set; }
        public int? LandlordId { get; set; }
        public Landlord? Landlord { get; set; }
        public int? TenantId { get; set; }
        public Tenant? Tenant { get; set; }
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }
        public string Email { get; set; }
        public string? RefreshToken { get; set; }
        public DateTime? RefreshTokenExpiryTime { get; set; }
    }
}

namespace RentEasyAPI.Models
{
    public class Landlord
    {
        public int LandlordId { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public ICollection<Property>? Properties { get; set; }
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }
        public string Role { get; set; }
        public string? RefreshToken { get; set; }
        public DateTime? RefreshTokenExpiryTime { get; set; }
    }
}

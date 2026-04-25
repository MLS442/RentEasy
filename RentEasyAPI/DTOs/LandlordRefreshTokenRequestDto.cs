namespace RentEasyAPI.DTOs
{
    public class LandlordRefreshTokenRequestDto
    {
        public int LandlordId { get; set; }
        public required string RefreshToken { get; set; }
    }
}

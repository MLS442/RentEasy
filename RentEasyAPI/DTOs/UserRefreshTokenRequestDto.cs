namespace RentEasyAPI.DTOs
{
    public class UserRefreshTokenRequestDto
    {
        public int UserId { get; set; }
        public required string RefreshToken { get; set; }
    }
}

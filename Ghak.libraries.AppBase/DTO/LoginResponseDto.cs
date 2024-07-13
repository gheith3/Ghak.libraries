namespace Ghak.libraries.AppBase.DTO;

public class LoginResponseDto
{
    public string Token { get; set; } = null!;
    public DateTime ExpiredAt { get; set; }

    public string RefreshToken { get; set; } = null!;
    public DateTime RefreshTokenExpiredAt { get; set; }
}
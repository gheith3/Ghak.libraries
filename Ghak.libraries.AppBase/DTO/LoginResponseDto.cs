namespace Ghak.libraries.AppBase.DTO;

public class LoginResponseDto
{
    public string Token { get; set; }
    public DateTime ExpiredAt { get; set; }

    public string RefreshToken { get; set; }
    public DateTime RefreshTokenExpiredAt { get; set; }
}
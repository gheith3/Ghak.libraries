namespace Ghak.libraries.AppBase.DTO;

public class LoginResponseDto
{
    public string Token { get; set; }
    public DateTime ExpiredAt { get; set; }
}
namespace Onion.Application.Services.Auth.Models;

public class RefreshTokenRes
{
    public string Token { get; set; }
    public bool IsRevoked { get; set; }
    public Guid UserId { get; set; }
}

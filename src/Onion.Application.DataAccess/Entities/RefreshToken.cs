namespace Onion.Application.DataAccess.Entities;

public class RefreshToken : BaseEntity
{
    public string Token { get; set; }
    public bool IsRevoked { get; set; } = false;
    public Guid UserId { get; set; }

    public User User { get; set; }

    public RefreshToken(string token, Guid userId)
    {
        Token = token;
        UserId = userId;
    }
}

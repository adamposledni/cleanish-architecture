namespace Onion.App.Data.Database.Entities;

public class RefreshToken : BaseEntity
{
    public string Token { get; set; }
    public bool IsRevoked { get; set; } = false;
    public DateTime Expiration { get; set; }
    public Guid UserId { get; set; }

    public User User { get; set; }

    public RefreshToken(string token, Guid userId, DateTime expiration)
    {
        Token = token;
        UserId = userId;
        Expiration = expiration;
    }

    public bool IsExpired(DateTime now) => Expiration <= now;
}

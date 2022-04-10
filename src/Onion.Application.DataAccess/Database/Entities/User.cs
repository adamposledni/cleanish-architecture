using Onion.Application.DataAccess.Database.Entities.Fields;

namespace Onion.Application.DataAccess.Database.Entities;

public class User : BaseEntity
{
    public string Email { get; set; }
    public byte[] PasswordHash { get; set; }
    public byte[] PasswordSalt { get; set; }
    public string GoogleSubjectId { get; set; }
    public UserRole Role { get; set; }
    public IEnumerable<RefreshToken> RefreshTokens { get; set; }
}

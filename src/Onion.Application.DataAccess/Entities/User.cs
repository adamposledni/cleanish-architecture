using System.Collections.Generic;

namespace Onion.Application.DataAccess.Entities
{
    public class User : BaseEntity
    {
        public string Email { get; set; }
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }
        public string GoogleSubjectId { get; set; }
        public IEnumerable<RefreshToken> RefreshTokens { get; set; }
    }
}

using System.Collections.Generic;
using System.Security.Claims;

namespace Onion.Core.Security
{
    public interface ITokenProvider
    {
        string GenerateJwt(IEnumerable<Claim> claims, int expiresIn);
        string GenerateRefreshToken();
    }
}

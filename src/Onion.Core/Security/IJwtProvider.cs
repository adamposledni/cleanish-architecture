using System.Collections.Generic;
using System.Security.Claims;

namespace Onion.Core.Security
{
    public interface IJwtProvider
    {
        string GenerateJwt(IEnumerable<Claim> claims);
    }
}

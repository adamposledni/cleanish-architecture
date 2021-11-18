using System.Collections.Generic;
using System.Security.Claims;

namespace Onion.Core.Security
{
    public interface IJwtService
    {
        string GenerateJwt(IEnumerable<Claim> claims);
    }
}

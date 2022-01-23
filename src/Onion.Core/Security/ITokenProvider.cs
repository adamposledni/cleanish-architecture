using System.Collections.Generic;
using System.Security.Claims;

namespace Onion.Core.Security
{
    public interface ITokenProvider
    {
        string GenerateAccessToken(IEnumerable<Claim> claims);
    }
}

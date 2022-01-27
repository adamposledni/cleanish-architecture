using System.Security.Claims;

namespace Onion.Core.Security;

public interface ITokenProvider
{
    string GenerateJwt(IEnumerable<Claim> claims, int expiresIn);
    bool IsTokenValid(string token);
}

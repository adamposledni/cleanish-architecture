using System.Security.Claims;

namespace Cleanish.App.Data.Security;

public interface IWebTokenService
{
    string CreateWebToken(IEnumerable<Claim> claims, int expiresIn);
    bool IsWebTokenValid(string token);
}
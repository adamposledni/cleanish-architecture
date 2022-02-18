using Microsoft.IdentityModel.Tokens;
using Onion.Core.Helpers;

namespace Onion.Infrastructure.Core.Security.Jwt;

public class TokenProviderSettings
{
    public string SigningKey { get; set; }

    public SecurityKey GetSecurityKey()
    {
        Guard.NotNullOrEmptyOrWhiteSpace(SigningKey, nameof(SigningKey));

        var key = Encoding.ASCII.GetBytes(SigningKey);
        return new SymmetricSecurityKey(key);
    }
}

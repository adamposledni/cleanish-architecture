using Cleanish.Shared.Helpers;
using Microsoft.IdentityModel.Tokens;

namespace Cleanish.Impl.Shared.Security.WebToken;

public class WebTokenSettings
{
    public const string CONFIG_KEY = "WebToken";

    public string SigningKey { get; set; }
    public string Audience { get; set; }
    public string Issuer { get; set; }

    public SecurityKey GetSecurityKey()
    {
        Guard.NotNullOrEmptyOrWhiteSpace(SigningKey, nameof(SigningKey));

        var key = Encoding.UTF8.GetBytes(SigningKey);
        return new SymmetricSecurityKey(key);
    }
}

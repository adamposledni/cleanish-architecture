using Microsoft.IdentityModel.Tokens;
using Onion.Shared.Helpers;

namespace Onion.Impl.App.Data.Security.WebToken;

public class WebTokenSettings
{
    public const string CONFIG_KEY = "WebToken";

    public string SigningKey { get; set; }

    public SecurityKey GetSecurityKey()
    {
        Guard.NotNullOrEmptyOrWhiteSpace(SigningKey, nameof(SigningKey));

        var key = Encoding.ASCII.GetBytes(SigningKey);
        return new SymmetricSecurityKey(key);
    }
}

using Microsoft.Extensions.Configuration;

namespace Onion.WebApi;

public static class Configuration
{
    private const string JWT_SIGNING_KEY = "JwtSettings:JwtSigningKey";

    public static string GetJwtSigningKey(this IConfiguration configuration)
    {
        return configuration.GetSection(JWT_SIGNING_KEY).Get<string>();
    }
}

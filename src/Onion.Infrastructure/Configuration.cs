using Microsoft.Extensions.Configuration;
using Onion.Infrastructure.Security.Google;
using Onion.Infrastructure.Security.Jwt;

namespace Onion.Infrastructure;

public static class Configuration
{
    private const string TOKEN_PROVIDER_SETTINGS = "JwtSettings";
    private const string GOOGLE_AUTH_SETTINGS = "GoogleAuthSettings";

    public static TokenProviderSettings GetTokenProviderSettings(this IConfiguration configuration)
    {
        return configuration.GetSection(TOKEN_PROVIDER_SETTINGS).Get<TokenProviderSettings>();
    }

    public static GoogleAuthSettings GetGoogleAuthSettings(this IConfiguration configuration)
    {
        return configuration.GetSection(GOOGLE_AUTH_SETTINGS).Get<GoogleAuthSettings>();

    }
}
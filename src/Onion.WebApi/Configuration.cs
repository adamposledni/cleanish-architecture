using Microsoft.Extensions.Configuration;
using Onion.Infrastructure.Security.Jwt;

namespace Onion.WebApi;

public static class Configuration
{
    private const string TOKEN_PROVIDER_SETTINGS = "TokenSettings";

    public static TokenProviderSettings GetTokenProviderSettings(this IConfiguration configuration)
    {
        return configuration.GetSection(TOKEN_PROVIDER_SETTINGS).Get<TokenProviderSettings>();
    }
}

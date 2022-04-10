using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Onion.Core.Cache;
using Onion.Core.Clock;
using Onion.Core.Mapper;
using Onion.Core.Security;
using Onion.Infrastructure.Core.Cache;
using Onion.Infrastructure.Core.Clock;
using Onion.Infrastructure.Core.Security.Google;
using Onion.Infrastructure.Core.Security.Jwt;
using Onion.Infrastructure.Core.Security.Password;
using M = Onion.Infrastructure.Core.Mapper;

namespace Onion.Infrastructure.Core;

public static class CoreLayer
{
    private const string TOKEN_PROVIDER_SETTINGS = "TokenProviderSettings";
    private const string GOOGLE_AUTH_SETTINGS = "GoogleAuthSettings";

    public static void Compose(IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IMapper, M.Mapper>();

        services.AddHttpClient();

        // TODO: cache size
        services.AddMemoryCache();
        services.AddScoped<ICacheService, CacheService>();

        services.Configure<TokenProviderSettings>(configuration.GetSection(TOKEN_PROVIDER_SETTINGS));
        services.AddScoped<ITokenProvider, TokenProvider>();
        
        services.AddScoped<IClockProvider, ClockProvider>();
        
        services.AddScoped<IPasswordProvider, PasswordProvider>();

        services.Configure<GoogleAuthSettings>(configuration.GetSection(GOOGLE_AUTH_SETTINGS));
        services.AddScoped<IGoogleAuthProvider, GoogleAuthProvider>();
    }
}
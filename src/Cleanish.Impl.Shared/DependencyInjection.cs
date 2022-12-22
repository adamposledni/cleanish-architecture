using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Cleanish.Impl.Shared.Clock;
using Cleanish.Shared.Clock;
using Cleanish.Impl.Shared.Security.Crypto;
using Cleanish.Impl.Shared.Security.WebToken;
using Cleanish.Shared.Security;

namespace Cleanish.Impl.Shared;

public static class DependencyInjection
{
    public static IServiceCollection AddShared(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IClockProvider, ClockProvider>();

        services
            .Configure<WebTokenSettings>(configuration.GetSection(WebTokenSettings.CONFIG_KEY))
            .AddTransient<IWebTokenService, WebTokenService>();

        services.AddTransient<ICryptographyService, CryptographyService>();

        return services;
    }
}
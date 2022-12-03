using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Cleanish.Impl.Shared.Clock;
using Cleanish.Shared.Clock;

namespace Cleanish.Impl.Shared;

public static class DependencyInjection
{
    public static IServiceCollection AddShared(this IServiceCollection services, IConfiguration _)
    {
        services.AddScoped<IClockProvider, ClockProvider>();

        return services;
    }
}
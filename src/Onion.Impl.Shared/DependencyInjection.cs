using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Onion.Impl.Shared.Clock;
using Onion.Shared.Clock;

namespace Onion.Impl.Shared;

public static class DependencyInjection
{
    public static IServiceCollection AddShared(this IServiceCollection services, IConfiguration _)
    {
        services.AddScoped<IClockProvider, ClockProvider>();

        return services;
    }
}
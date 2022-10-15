using Mapster;
using MapsterMapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Onion.Impl.Shared.Clock;
using Onion.Impl.Shared.Mapper;
using Onion.Shared.Clock;
using Onion.Shared.Mapper;

namespace Onion.Impl.Shared;

public static class DependencyInjection
{
    public static IServiceCollection AddShared(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton(TypeAdapterConfig.GlobalSettings);
        services.AddScoped<IMapper, ServiceMapper>();
        services.AddScoped<IObjectMapper, ObjectMapper>();

        services.AddScoped<IClockProvider, ClockProvider>();

        return services;
    }
}
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Onion.Core.Clock;
using Onion.Core.Mapper;
using Onion.Core.Security;
using Onion.Infrastructure.Clock;
using Onion.Infrastructure.Security.Google;
using Onion.Infrastructure.Security.Jwt;
using Onion.Infrastructure.Security.Password;
using M = Onion.Infrastructure.Mapper;

namespace Onion.Infrastructure;

public static class CoreLayer
{
    public static void Compose(IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IMapper, M.Mapper>();
        services.AddScoped<ITokenProvider, TokenProvider>();
        services.AddScoped<IClockProvider, ClockProvider>();
        services.AddScoped<IPasswordProvider, PasswordProvider>();
        services.AddScoped<IGoogleAuthProvider, GoogleAuthProvider>();
    }
}
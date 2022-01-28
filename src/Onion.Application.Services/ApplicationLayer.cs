using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Onion.Application.Services.Auth;
using Onion.Application.Services.UserManagement;

namespace Onion.Application.Services;

public static class ApplicationLayer
{
    public static void Compose(IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IAuthService, AuthService>();
    }
}
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Onion.Application.Services.Auth;
using Onion.Application.Services.Common;
using Onion.Application.Services.TodoLists;
using Onion.Application.Services.Users;

namespace Onion.Application.Services;

public static class ApplicationLayer
{
    private const string APPLICATION_SETTINGS = "ApplicationSettings";

    public static void Compose(IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<ApplicationSettings>(configuration.GetSection(APPLICATION_SETTINGS));

        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<ITodoItemService, TodoItemService>();
        services.AddScoped<ITodoListService, TodoListService>();
    }
}
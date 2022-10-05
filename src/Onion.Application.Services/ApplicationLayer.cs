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

        services.AddTransient<IUserService, UserService>();
        services.AddTransient<IAuthService, AuthService>();
        services.AddTransient<ITodoItemService, TodoItemService>();
        services.AddTransient<ITodoListService, TodoListService>();
    }
}
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Onion.App.Logic.Auth;
using Onion.App.Logic.Common;
using Onion.App.Logic.Common.Mediator.Behaviors;
using Onion.App.Logic.TodoLists;
using Onion.App.Logic.Users;
using System.Reflection;

namespace Onion.App.Logic;

public static class DependencyInjection
{
    public static IServiceCollection AddApplicationLogic(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<ApplicationSettings>(configuration.GetSection(ApplicationSettings.CONFIG_KEY));

        ValidatorOptions.Global.LanguageManager.Enabled = false;
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

        services
            .AddMediatR(Assembly.GetExecutingAssembly())
            .AddTransient(typeof(IPipelineBehavior<,>), typeof(AuthorizationBehavior<,>))
            .AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

        services
            .AddTransient<IAuthService, AuthService>()
            .AddTransient<ITodoItemService, TodoItemService>()
            .AddTransient<ITodoListService, TodoListService>();

        return services;
    }
}
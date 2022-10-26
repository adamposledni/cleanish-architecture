using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Onion.App.Data.Cache;
using Onion.App.Data.Database.Entities;
using Onion.App.Data.Database.Repositories;
using Onion.App.Data.Security;
using Onion.Impl.App.Data.Cache;
using Onion.Impl.App.Data.Database;
using Onion.Impl.App.Data.Database.Repositories;
using Onion.Impl.App.Data.Security.Crypto;
using Onion.Impl.App.Data.Security.WebToken;

namespace Onion.Impl.App.Data;

public static class DependencyInjection
{
    public static IServiceCollection AddApplicationData(this IServiceCollection services, IConfiguration configuration)
    {
        var cacheSettings = configuration.GetSection(CacheSettings.CONFIG_KEY).Get<CacheSettings>();
        services
            .Configure<CacheSettings>(configuration.GetSection(CacheSettings.CONFIG_KEY))
            .AddMemoryCache(o => o.SizeLimit = cacheSettings.Size);

        services.AddDbContext<SqlDbContext>(opt => opt.UseSqlServer(configuration.GetConnectionString("Sql")));

        services
            .AddDatabaseRepository<User, IUserRepository, UserRepository>()
            .AddDatabaseRepository<TodoList, ITodoListRepository, TodoListRepository>()
            .AddDatabaseRepository<TodoItem, ITodoItemRepository, TodoItemRepository>()
            .AddDatabaseRepository<RefreshToken, IRefreshTokenRepository, RefreshTokenRepository>();

        services
            .Configure<WebTokenSettings>(configuration.GetSection(WebTokenSettings.CONFIG_KEY))
            .AddTransient<IWebTokenService, WebTokenService>();

        services.AddTransient<ICryptographyService, CryptographyService>();

        return services;
    }

    private static IServiceCollection AddDatabaseRepository<TEntity, TService, TImplementation>(this IServiceCollection services) where TImplementation : TService where TService : ICachable
    {
        services.AddSingleton<ICacheService<TEntity>, CacheService<TEntity>>();

        services.AddTransient(typeof(TService), sp =>
        {
            return Activator.CreateInstance(
                typeof(TImplementation),
                sp.GetRequiredService<SqlDbContext>(),
                sp.GetRequiredService<ICacheService<TEntity>>(),
                CacheStrategy.Bypass);
        });

        services.AddTransient(typeof(Cached<TService>), sp =>
        {
            var repository = (TService)Activator.CreateInstance(
                typeof(TImplementation),
                sp.GetRequiredService<SqlDbContext>(),
                sp.GetRequiredService<ICacheService<TEntity>>(),
                CacheStrategy.Use);
            return new Cached<TService>(repository);
        });

        return services;
    }
}
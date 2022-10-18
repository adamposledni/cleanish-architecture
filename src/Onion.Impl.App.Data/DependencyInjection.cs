using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Onion.App.Data.Cache;
using Onion.App.Data.Database.Repositories;
using Onion.App.Data.Security;
using Onion.Impl.App.Data.Cache;
using Onion.Impl.App.Data.Database;
using Onion.Impl.App.Data.Database.Repositories;
using Onion.Impl.App.Data.Security.Crypto;
using Onion.Impl.App.Data.Security.Google;
using Onion.Impl.App.Data.Security.WebToken;

namespace Onion.Impl.App.Data;

public delegate object DatabaseRepositoryProvider(Type entityType, Type repositoryType, CacheStrategy cacheStrategy);

public static class DependencyInjection
{
    public static IServiceCollection AddApplicationData(this IServiceCollection services, IConfiguration configuration)
    {
        var cacheSettings = configuration.GetSection(CacheSettings.CONFIG_KEY).Get<CacheSettings>();
        services
            .Configure<CacheSettings>(configuration.GetSection(CacheSettings.CONFIG_KEY))
            .AddMemoryCache(o => o.SizeLimit = cacheSettings.Size)
            .AddTransient<ICacheService, CacheService>();

        services.AddDbContext<SqlDbContext>(opt => opt.UseSqlServer(configuration.GetConnectionString("Sql")));

        services
            .AddDatabaseRepository<IUserRepository, UserRepository>()
            .AddDatabaseRepository<IUserRepository, UserRepository>()
            .AddDatabaseRepository<ITodoListRepository, TodoListRepository>()
            .AddDatabaseRepository<ITodoItemRepository, TodoItemRepository>()
            .AddDatabaseRepository<IRefreshTokenRepository, RefreshTokenRepository>();

        services
            .Configure<WebTokenSettings>(configuration.GetSection(WebTokenSettings.CONFIG_KEY))
            .AddTransient<IWebTokenService, WebTokenService>();

        services.AddTransient<ICryptographyService, CryptographyService>();

        services
            .Configure<GoogleAuthSettings>(configuration.GetSection(GoogleAuthSettings.CONFIG_KEY))
            .AddTransient<IGoogleAuthProvider, GoogleAuthProvider>();

        return services;
    }

    private static IServiceCollection AddDatabaseRepository<TService, TImplementation>(this IServiceCollection services) where TImplementation : TService where TService : ICachable
    {
        services.AddTransient(typeof(TService), sp =>
        {
            return Activator.CreateInstance(
                typeof(TImplementation),
                sp.GetRequiredService<SqlDbContext>(),
                sp.GetRequiredService<ICacheService>(),
                CacheStrategy.Bypass);
        });

        services.AddTransient(typeof(Cached<TService>), sp =>
        {
            var repository = (TService)Activator.CreateInstance(
                typeof(TImplementation),
                sp.GetRequiredService<SqlDbContext>(),
                sp.GetRequiredService<ICacheService>(),
                CacheStrategy.Use);
            return new Cached<TService>(repository);
        });

        return services;
    }
}
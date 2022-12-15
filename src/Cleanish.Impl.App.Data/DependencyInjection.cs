using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Cleanish.App.Data.Cache;
using Cleanish.App.Data.Database.Entities;
using Cleanish.App.Data.Database.Repositories;
using Cleanish.App.Data.Security;
using Cleanish.Impl.App.Data.Cache;
using Cleanish.Impl.App.Data.Database;
using Cleanish.Impl.App.Data.Database.Repositories;
using Cleanish.Impl.App.Data.Security.Crypto;
using Cleanish.Impl.App.Data.Security.WebToken;

namespace Cleanish.Impl.App.Data;

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
            .AddDatabaseRepository<TodoItem, ITodoItemRepository, TodoItemRepository>();

        services
            .Configure<WebTokenSettings>(configuration.GetSection(WebTokenSettings.CONFIG_KEY))
            .AddTransient<IWebTokenService, WebTokenService>();

        services.AddTransient<ICryptographyService, CryptographyService>();

        return services;
    }

    private static IServiceCollection AddDatabaseRepository<TEntity, TService, TImplementation>(this IServiceCollection services) 
        where TEntity: BaseEntity
        where TImplementation : TService 
        where TService : ICachable
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
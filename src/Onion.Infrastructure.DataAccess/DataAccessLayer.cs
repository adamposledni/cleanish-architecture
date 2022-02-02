using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Onion.Application.DataAccess.Repositories;
using Onion.Infrastructure.DataAccess.MongoDb;
using Onion.Infrastructure.DataAccess.Sql;
using Onion.Infrastructure.DataAccess.Sql.Repositories;

namespace Onion.Infrastructure.DataAccess;

public static class DataAccessLayer
{
    private const string MONGO_DB_SETTINGS = "MongoDbSettings:Default";

    public static void Compose(IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<SqlDbContext>(opt =>
        {
            opt.UseSqlServer(configuration.GetConnectionString("Default"));
        });

        services.Configure<MongoDbSettings>(configuration.GetSection(MONGO_DB_SETTINGS));
        MongoDbContext.Configure();
        services.AddScoped<IMongoDbContext, MongoDbContext>();

        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();
    }
}
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
    public static void Compose(IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<SqlDbContext>(opt => opt.UseSqlServer(configuration.GetConnectionString("Sql")));

        MongoDbContext.Configure();
        services.AddScoped<IMongoDbContext, MongoDbContext>()
                .AddScoped(sp => new MongoDbContextOptions(configuration.GetConnectionString("MongoDb")));

        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();
    }
}
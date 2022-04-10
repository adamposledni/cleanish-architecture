using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Onion.Application.DataAccess.Database;
using Onion.Application.DataAccess.Database.Repositories;
using Onion.Infrastructure.DataAccess.Database;
using Onion.Infrastructure.DataAccess.Database.Repositories;

namespace Onion.Infrastructure.DataAccess;

public static class DataAccessLayer
{
    public static void Compose(IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<SqlDbContext>(opt => opt.UseSqlServer(configuration.GetConnectionString("Sql")));

        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();
    }
}
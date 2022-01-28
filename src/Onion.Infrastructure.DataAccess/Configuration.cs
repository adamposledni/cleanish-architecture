using Microsoft.Extensions.Configuration;
using Onion.Infrastructure.DataAccess.MongoDb;

namespace Onion.Infrastructure.DataAccess;

public static class Configuration
{
    private const string MONGO_DB_SETTINGS = "MongoDbSettings:Default";

    public static MongoDbSettings GetMongoDbSettings(this IConfiguration configuration)
    {
        return configuration.GetSection(MONGO_DB_SETTINGS).Get<MongoDbSettings>();
    }
}
using Microsoft.Extensions.Configuration;
using Onion.Infrastucture.DataAccess.MongoDb.Configuration;

namespace Onion.WebApi.Extensions
{
    public static class IConfigurationExtensions
    {
        public static MongoDbSettings GetMongoDbSettings(this IConfiguration configuration, string name = "Default")
        {
            return configuration.GetSection($"MongoDbSettings:{name}").Get<MongoDbSettings>();
        }

        public static IConfigurationSection GetApplicationSettingsSection(this IConfiguration configuration)
        {
            return configuration.GetSection("ApplicationSettings");
        }

        public static string GetJwtSigningKey(this IConfiguration configuration)
        {
            return configuration.GetValue<string>("ApplicationSettings:JwtSigningKey");
        }
    }
}

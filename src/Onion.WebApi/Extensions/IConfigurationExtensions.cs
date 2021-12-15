using Microsoft.Extensions.Configuration;
using Onion.Infrastucture.DataAccess.MongoDb;

namespace Onion.WebApi.Extensions
{
    public static class IConfigurationExtensions
    {
        public static IConfigurationSection GetMongoDbSettingsSection(this IConfiguration configuration, string name = "Default")
        {
            return configuration.GetSection($"MongoDbSettings:{name}");
        }

        public static IConfigurationSection GetApplicationSettingsSection(this IConfiguration configuration)
        {
            return configuration.GetSection("ApplicationSettings");
        }

        public static IConfigurationSection GetJwtSettingsSection(this IConfiguration configuration)
        {
            return configuration.GetSection("JwtSettings");
        }

        public static IConfigurationSection GetGoogleAuthSettingsSection(this IConfiguration configuration)
        {
            return configuration.GetSection("GoogleAuthSettings");
        }

        public static IConfigurationSection GetFacebookAuthSettingsSection(this IConfiguration configuration)
        {
            return configuration.GetSection("FacebookAuthSettings");
        }

        public static string GetJwtSigningKey(this IConfiguration configuration)
        {
            return configuration.GetValue<string>("JwtSettings:JwtSigningKey");
        }
    }
}

using Microsoft.Extensions.Configuration;

namespace Onion.WebApi.Extensions;

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

    public static string GetJwtSigningKey(this IConfiguration configuration)
    {
        return configuration.GetValue<string>("JwtSettings:JwtSigningKey");
    }
}

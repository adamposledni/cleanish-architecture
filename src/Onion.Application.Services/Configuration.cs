using Microsoft.Extensions.Configuration;
using Onion.Application.Services.Common;

namespace Onion.Application.Services;

public static class Configuration
{
    private const string APPLICATION_SETTINGS = "ApplicationSettings";

    public static ApplicationSettings GetApplicationSettings(this IConfiguration configuration)
    {
        return configuration.GetSection(APPLICATION_SETTINGS).Get<ApplicationSettings>();
    }
}
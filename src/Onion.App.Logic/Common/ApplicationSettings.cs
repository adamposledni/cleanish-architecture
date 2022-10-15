namespace Onion.App.Logic.Common;

public class ApplicationSettings
{
    public const string CONFIG_KEY = "Application";

    public int AccessTokenLifetime { get; set; }
    public int RefreshTokenLifetime { get; set; }
}
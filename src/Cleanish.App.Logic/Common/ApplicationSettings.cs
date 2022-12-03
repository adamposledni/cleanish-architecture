namespace Cleanish.App.Logic.Common;

internal class ApplicationSettings
{
    public const string CONFIG_KEY = "Application";

    public int AccessTokenLifetime { get; set; }
    public int RefreshTokenLifetime { get; set; }
}
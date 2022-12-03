namespace Cleanish.Shared.Exceptions;

public class NotAuthenticatedException : Exception
{
    public string MessageKey { get; init; } = "NotAuthenticated";

    public NotAuthenticatedException() : base()
    {
    }
}
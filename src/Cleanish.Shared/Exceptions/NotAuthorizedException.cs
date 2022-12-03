namespace Cleanish.Shared.Exceptions;

public class NotAuthorizedException : Exception
{
    public string MessageKey { get; init; } = "NotAuthorized";

    public NotAuthorizedException() : base()
    {
    }
}

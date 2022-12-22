namespace Cleanish.Shared.Exceptions;

public class NotAuthenticatedException : BaseApplicationException
{
    public NotAuthenticatedException() : base("NotAuthenticated") { }
}
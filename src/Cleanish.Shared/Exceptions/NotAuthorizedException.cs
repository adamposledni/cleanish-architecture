namespace Cleanish.Shared.Exceptions;

public class NotAuthorizedException : BaseApplicationException
{
    public NotAuthorizedException() : base("NotAuthorized") { }
}

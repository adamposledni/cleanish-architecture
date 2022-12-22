namespace Cleanish.Shared.Exceptions;

public abstract class BadLogicException : BaseApplicationException
{
    public BadLogicException(string messageKey) : base(messageKey) { }
}

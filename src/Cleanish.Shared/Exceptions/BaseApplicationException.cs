namespace Cleanish.Shared.Exceptions;

public abstract class BaseApplicationException : Exception
{
    public string MessageKey { get; init; }

    public BaseApplicationException(string messageKey)
    {
        MessageKey= messageKey;
    }
}

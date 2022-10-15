namespace Onion.Shared.Exceptions;

public abstract class BadLogicException : Exception
{
    public string MessageKey { get; private set; }

    public BadLogicException(string messageKey) : base()
    {
        MessageKey = messageKey;
    }
}

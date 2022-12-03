namespace Cleanish.Shared.Exceptions;

public abstract class NotFoundException : Exception
{
    public string MessageKey { get; private set; }

    public NotFoundException(string messageKey) : base()
    {
        MessageKey = messageKey;
    }
}

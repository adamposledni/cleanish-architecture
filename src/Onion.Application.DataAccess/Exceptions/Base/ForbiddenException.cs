namespace Onion.Application.DataAccess.Exceptions.Base;

public class ForbiddenException : Exception
{
    private const string MESSAGE_KEY = "Forbidden";
    public string MessageKey { get; private set; }

    public ForbiddenException() : base()
    {
        MessageKey = MESSAGE_KEY;
    }
}

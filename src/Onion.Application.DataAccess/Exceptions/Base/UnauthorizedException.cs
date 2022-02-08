namespace Onion.Application.DataAccess.Exceptions.Base;

public class UnauthorizedException : Exception
{
    private const string MESSAGE_KEY = "Unauthorized";
    public string MessageKey { get; private set; }

    public UnauthorizedException() : base()
    {
        MessageKey = MESSAGE_KEY;
    }
}

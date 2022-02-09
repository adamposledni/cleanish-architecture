using Onion.Application.DataAccess.BaseExceptions;

namespace Onion.Application.Services.Auth.Exceptions;

public class InvalidEmailPasswordException : BadRequestException
{
    private const string MESSAGE_KEY = "InvalidEmailPassword";
    public InvalidEmailPasswordException() : base(MESSAGE_KEY)
    {
    }
}

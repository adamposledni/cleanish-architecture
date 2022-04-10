using Onion.Core.Exceptions;

namespace Onion.Application.Services.Auth.Exceptions;

public class InvalidGoogleIdTokenException : BadRequestException
{
    private const string MESSAGE_KEY = "InvalidGoogleIdToken";

    public InvalidGoogleIdTokenException() : base(MESSAGE_KEY)
    {
    }
}

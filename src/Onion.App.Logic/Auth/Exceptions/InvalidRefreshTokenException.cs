using Onion.Shared.Exceptions;

namespace Onion.App.Logic.Auth.Exceptions;

public class InvalidRefreshTokenException : BadLogicException
{
    public InvalidRefreshTokenException() : base("InvalidRefreshToken")
    {
    }
}

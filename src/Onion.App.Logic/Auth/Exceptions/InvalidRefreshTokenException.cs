using Onion.Shared.Exceptions;

namespace Onion.App.Logic.Auth.Exceptions;

internal class InvalidRefreshTokenException : BadLogicException
{
    public InvalidRefreshTokenException() : base("InvalidRefreshToken")
    {
    }
}

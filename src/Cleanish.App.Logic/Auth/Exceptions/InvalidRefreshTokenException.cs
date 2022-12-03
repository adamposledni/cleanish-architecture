using Cleanish.Shared.Exceptions;

namespace Cleanish.App.Logic.Auth.Exceptions;

internal class InvalidRefreshTokenException : BadLogicException
{
    public InvalidRefreshTokenException() : base("InvalidRefreshToken")
    {
    }
}

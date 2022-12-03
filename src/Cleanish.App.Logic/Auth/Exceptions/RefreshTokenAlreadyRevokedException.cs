using Cleanish.Shared.Exceptions;

namespace Cleanish.App.Logic.Auth.Exceptions;

internal class RefreshTokenAlreadyRevokedException : BadLogicException
{
    public RefreshTokenAlreadyRevokedException() : base("RefreshTokenAlreadyRevoked")
    {
    }
}

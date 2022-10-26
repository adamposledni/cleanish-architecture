using Onion.Shared.Exceptions;

namespace Onion.App.Logic.Auth.Exceptions;

internal class RefreshTokenAlreadyRevokedException : BadLogicException
{
    public RefreshTokenAlreadyRevokedException() : base("RefreshTokenAlreadyRevoked")
    {
    }
}

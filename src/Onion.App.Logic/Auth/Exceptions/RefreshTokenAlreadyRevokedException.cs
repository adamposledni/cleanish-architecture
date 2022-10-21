using Onion.Shared.Exceptions;

namespace Onion.App.Logic.Auth.Exceptions;

public class RefreshTokenAlreadyRevokedException : BadLogicException
{
    public RefreshTokenAlreadyRevokedException() : base("RefreshTokenAlreadyRevoked")
    {
    }
}

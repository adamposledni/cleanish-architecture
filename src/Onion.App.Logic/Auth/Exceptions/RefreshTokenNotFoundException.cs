using Onion.Shared.Exceptions;

namespace Onion.App.Logic.Auth.Exceptions;

internal class RefreshTokenNotFoundException : NotFoundException
{
    public RefreshTokenNotFoundException() : base("RefreshTokenNotFound")
    {
    }
}

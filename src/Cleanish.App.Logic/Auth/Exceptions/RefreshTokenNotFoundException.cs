using Cleanish.Shared.Exceptions;

namespace Cleanish.App.Logic.Auth.Exceptions;

internal class RefreshTokenNotFoundException : NotFoundException
{
    public RefreshTokenNotFoundException() : base("RefreshTokenNotFound")
    {
    }
}

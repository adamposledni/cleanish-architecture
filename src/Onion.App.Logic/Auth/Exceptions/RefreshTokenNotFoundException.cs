using Onion.Shared.Exceptions;

namespace Onion.App.Logic.Auth.Exceptions;

public class RefreshTokenNotFoundException : NotFoundException
{
    public RefreshTokenNotFoundException() : base("RefreshTokenNotFound")
    {
    }
}

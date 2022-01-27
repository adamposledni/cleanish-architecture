using Onion.Application.DataAccess.Exceptions.Common;

namespace Onion.Application.DataAccess.Exceptions.RefreshToken;

public class RefreshTokenNotFoundException : NotFoundException
{
    private const string MESSAGE_KEY = "RefreshTokenNotFound";

    public RefreshTokenNotFoundException() : base(MESSAGE_KEY)
    {
    }
}

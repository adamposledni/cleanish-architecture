using Onion.Application.DataAccess.Exceptions.Common;

namespace Onion.Application.DataAccess.Exceptions.RefreshToken;

public class RefreshTokenAlreadyRevokedException : BadRequestException
{
    private const string MESSAGE_KEY = "RefreshTokenAlreadyRevoked";

    public RefreshTokenAlreadyRevokedException() : base(MESSAGE_KEY)
    {
    }
}

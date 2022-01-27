using Onion.Application.DataAccess.Exceptions.Common;

namespace Onion.Application.DataAccess.Exceptions.RefreshToken;

public class InvalidRefreshTokenException : BadRequestException
{
    private const string MESSAGE_KEY = "InvalidRefreshToken";
    public InvalidRefreshTokenException() : base(MESSAGE_KEY)
    {
    }
}

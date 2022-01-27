using Onion.Application.DataAccess.Exceptions.Common;

namespace Onion.Application.DataAccess.Exceptions.User;

public class UserNotFoundException : NotFoundException
{
    private const string MESSAGE_KEY = "UserNotFound";
    public UserNotFoundException() : base(MESSAGE_KEY)
    {
    }
}

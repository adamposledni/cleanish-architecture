using Onion.Core.Exceptions;

namespace Onion.Application.Services.Users.Exceptions;

public class UserNotFoundException : NotFoundException
{
    private const string MESSAGE_KEY = "UserNotFound";
    public UserNotFoundException() : base(MESSAGE_KEY)
    {
    }
}

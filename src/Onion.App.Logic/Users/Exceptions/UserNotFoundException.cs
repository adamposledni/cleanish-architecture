using Onion.Shared.Exceptions;

namespace Onion.App.Logic.Users.Exceptions;

public class UserNotFoundException : NotFoundException
{
    public UserNotFoundException() : base("UserNotFound")
    {
    }
}

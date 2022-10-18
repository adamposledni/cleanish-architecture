using Onion.Shared.Exceptions;

namespace Onion.App.Logic.Users.Exceptions;

internal class UserNotFoundException : NotFoundException
{
    public UserNotFoundException() : base("UserNotFound")
    {
    }
}

using Cleanish.Shared.Exceptions;

namespace Cleanish.App.Logic.Users.Exceptions;

internal class UserNotFoundException : NotFoundException
{
    public UserNotFoundException() : base("UserNotFound")
    {
    }
}

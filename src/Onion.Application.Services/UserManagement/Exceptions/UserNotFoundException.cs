using Onion.Application.DataAccess.BaseExceptions;

namespace Onion.Application.Services.UserManagement.Exceptions;

public class UserNotFoundException : NotFoundException
{
    private const string MESSAGE_KEY = "UserNotFound";
    public UserNotFoundException() : base(MESSAGE_KEY)
    {
    }
}

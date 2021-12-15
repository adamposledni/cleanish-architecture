using System;

namespace Onion.Application.DataAccess.Exceptions
{
    public class UserNotFoundException : NotFoundException
    {
        private const string MESSAGE_KEY = "UserNotFoundMessage";
        public UserNotFoundException(Guid itemId) : base(itemId, MESSAGE_KEY)
        {
        }
    }
}

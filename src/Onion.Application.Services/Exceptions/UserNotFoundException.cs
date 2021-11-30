using System;

namespace Onion.Application.Services.Exceptions
{
    public class UserNotFoundException : NotFoundException
    {
        private const string MESSAGE_KEY = "UserNotFoundMessage";
        public UserNotFoundException(Guid itemId) : base(itemId, MESSAGE_KEY)
        {
        }
    }
}

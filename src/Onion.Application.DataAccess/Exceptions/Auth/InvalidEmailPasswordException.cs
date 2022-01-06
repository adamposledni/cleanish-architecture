using Onion.Application.DataAccess.Exceptions.Common;

namespace Onion.Application.DataAccess.Exceptions.Auth
{
    public class InvalidEmailPasswordException : BadRequestException
    {
        private const string MESSAGE_KEY = "InvalidEmailPassword";
        public InvalidEmailPasswordException() : base(MESSAGE_KEY)
        {
        }
    }
}

using Onion.Application.DataAccess.Exceptions.Common;

namespace Onion.Application.DataAccess.Exceptions.Auth
{
    public class InvalidGoogleIdTokenException : BadRequestException
    {
        private const string MESSAGE_KEY = "InvalidGoogleIdToken";

        public InvalidGoogleIdTokenException() : base(MESSAGE_KEY)
        {
        }
    }
}

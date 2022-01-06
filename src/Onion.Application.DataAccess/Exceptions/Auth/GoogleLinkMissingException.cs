using Onion.Application.DataAccess.Exceptions.Common;

namespace Onion.Application.DataAccess.Exceptions.Auth
{
    public class GoogleLinkMissingException : BadRequestException
    {
        private const string MESSAGE_KEY = "GoogleLinkMissing";

        public GoogleLinkMissingException() : base(MESSAGE_KEY)
        {
        }
    }
}

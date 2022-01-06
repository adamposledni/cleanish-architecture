namespace Onion.Application.DataAccess.Exceptions.Common
{
    public class InvalidRequestException : BadRequestException
    {
        private const string MESSAGE_KEY = "InvalidRequest";

        public InvalidRequestException(string details) : base(MESSAGE_KEY, details)
        {
        }
    }
}

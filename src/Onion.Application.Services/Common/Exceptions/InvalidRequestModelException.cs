using Onion.Application.DataAccess.Exceptions.Base;

namespace Onion.Application.Services.Common.Exceptions;

public class InvalidRequestModelException : BadRequestException
{
    private const string MESSAGE_KEY = "InvalidRequest";

    public InvalidRequestModelException(string details) : base(MESSAGE_KEY, details)
    {
    }
}

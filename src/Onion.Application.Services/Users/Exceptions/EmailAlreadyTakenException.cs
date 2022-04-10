using Onion.Core.Exceptions;

namespace Onion.Application.Services.Users.Exceptions;

public class EmailAlreadyTakenException : BadRequestException
{
    private const string MESSAGE_KEY = "EmailAlreadyTaken";
    public EmailAlreadyTakenException() : base(MESSAGE_KEY)
    {
    }
}
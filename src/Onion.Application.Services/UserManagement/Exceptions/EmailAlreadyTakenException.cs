using Onion.Application.DataAccess.BaseExceptions;

namespace Onion.Application.Services.UserManagement.Exceptions;

public class EmailAlreadyTakenException : BadRequestException
{
    private const string MESSAGE_KEY = "EmailAlreadyTaken";
    public EmailAlreadyTakenException() : base(MESSAGE_KEY)
    {
    }
}
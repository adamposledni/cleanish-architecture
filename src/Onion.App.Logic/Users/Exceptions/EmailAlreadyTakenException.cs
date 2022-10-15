using Onion.Shared.Exceptions;

namespace Onion.App.Logic.Users.Exceptions;

public class EmailAlreadyTakenException : BadLogicException
{
    public EmailAlreadyTakenException() : base("EmailAlreadyTaken")
    {
    }
}
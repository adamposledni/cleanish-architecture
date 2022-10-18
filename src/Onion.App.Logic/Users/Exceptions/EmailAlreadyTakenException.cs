using Onion.Shared.Exceptions;

namespace Onion.App.Logic.Users.Exceptions;

internal class EmailAlreadyTakenException : BadLogicException
{
    public EmailAlreadyTakenException() : base("EmailAlreadyTaken")
    {
    }
}
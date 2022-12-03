using Cleanish.Shared.Exceptions;

namespace Cleanish.App.Logic.Users.Exceptions;

internal class EmailAlreadyTakenException : BadLogicException
{
    public EmailAlreadyTakenException() : base("EmailAlreadyTaken")
    {
    }
}
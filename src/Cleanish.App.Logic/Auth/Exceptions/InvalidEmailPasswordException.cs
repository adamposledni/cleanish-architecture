using Cleanish.Shared.Exceptions;

namespace Cleanish.App.Logic.Auth.Exceptions;

internal class InvalidEmailPasswordException : BadLogicException
{
    public InvalidEmailPasswordException() : base("InvalidEmailPassword")
    {
    }
}

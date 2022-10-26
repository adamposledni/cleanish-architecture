using Onion.Shared.Exceptions;

namespace Onion.App.Logic.Auth.Exceptions;

internal class InvalidEmailPasswordException : BadLogicException
{
    public InvalidEmailPasswordException() : base("InvalidEmailPassword")
    {
    }
}

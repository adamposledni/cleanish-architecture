using Onion.Shared.Exceptions;

namespace Onion.App.Logic.Auth.Exceptions;

public class InvalidEmailPasswordException : BadLogicException
{
    public InvalidEmailPasswordException() : base("InvalidEmailPassword")
    {
    }
}

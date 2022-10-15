using Onion.Shared.Exceptions;

namespace Onion.App.Logic.Auth.Exceptions;

public class InvalidGoogleIdTokenException : BadLogicException
{
    public InvalidGoogleIdTokenException() : base("InvalidGoogleIdToken")
    {
    }
}

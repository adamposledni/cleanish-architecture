using Onion.Shared.Exceptions;

namespace Onion.App.Logic.Auth.Exceptions;

internal class InvalidGoogleIdTokenException : BadLogicException
{
    public InvalidGoogleIdTokenException() : base("InvalidGoogleIdToken")
    {
    }
}

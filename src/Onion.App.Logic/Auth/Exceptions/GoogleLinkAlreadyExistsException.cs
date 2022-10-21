using Onion.Shared.Exceptions;

namespace Onion.App.Logic.Auth.Exceptions;

public class GoogleLinkAlreadyExistsException : BadLogicException
{
    public GoogleLinkAlreadyExistsException() : base("GoogleLinkAlreadyExists")
    {
    }
}

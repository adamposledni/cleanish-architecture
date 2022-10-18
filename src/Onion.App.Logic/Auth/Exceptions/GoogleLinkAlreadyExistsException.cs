using Onion.Shared.Exceptions;

namespace Onion.App.Logic.Auth.Exceptions;

internal class GoogleLinkAlreadyExistsException : BadLogicException
{
    public GoogleLinkAlreadyExistsException() : base("GoogleLinkAlreadyExists")
    {
    }
}

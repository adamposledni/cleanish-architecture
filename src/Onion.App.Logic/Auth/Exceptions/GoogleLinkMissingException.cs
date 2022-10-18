using Onion.Shared.Exceptions;

namespace Onion.App.Logic.Auth.Exceptions;

internal class GoogleLinkMissingException : BadLogicException
{
    public GoogleLinkMissingException() : base("GoogleLinkMissing")
    {
    }
}

using Onion.Shared.Exceptions;

namespace Onion.App.Logic.Auth.Exceptions;

public class GoogleLinkMissingException : BadLogicException
{
    public GoogleLinkMissingException() : base("GoogleLinkMissing")
    {
    }
}

using Onion.Core.Exceptions;

namespace Onion.Application.Services.Auth.Exceptions;

public class GoogleLinkMissingException : BadRequestException
{
    private const string MESSAGE_KEY = "GoogleLinkMissing";

    public GoogleLinkMissingException() : base(MESSAGE_KEY)
    {
    }
}

using Onion.Core.Exceptions;

namespace Onion.Application.Services.Auth.Exceptions;

public class GoogleLinkAlreadyExistsException : BadRequestException
{
    private const string MESSAGE_KEY = "GoogleLinkAlreadyExists";

    public GoogleLinkAlreadyExistsException() : base(MESSAGE_KEY)
    {
    }
}

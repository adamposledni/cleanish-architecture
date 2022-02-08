using Onion.Application.DataAccess.Exceptions.Base;

namespace Onion.Application.DataAccess.Exceptions;

public class PageOutOfBoundsException : BadRequestException
{
    private const string MESSAGE_KEY = "PageOutOfBounds";
    public PageOutOfBoundsException() : base(MESSAGE_KEY)
    {
    }
}

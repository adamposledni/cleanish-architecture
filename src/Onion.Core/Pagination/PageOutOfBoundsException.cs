using Onion.Core.Exceptions;

namespace Onion.Core.Pagination;

public class PageOutOfBoundsException : BadRequestException
{
    private const string MESSAGE_KEY = "PageOutOfBounds";
    public PageOutOfBoundsException() : base(MESSAGE_KEY)
    {
    }
}

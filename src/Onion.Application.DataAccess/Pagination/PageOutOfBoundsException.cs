using Onion.Application.DataAccess.BaseExceptions;

namespace Onion.Application.DataAccess.Pagination;

public class PageOutOfBoundsException : BadRequestException
{
    private const string MESSAGE_KEY = "PageOutOfBounds";
    public PageOutOfBoundsException() : base(MESSAGE_KEY)
    {
    }
}

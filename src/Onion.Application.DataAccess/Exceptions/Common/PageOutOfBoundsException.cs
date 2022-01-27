namespace Onion.Application.DataAccess.Exceptions.Common;

public class PageOutOfBoundsException : BadRequestException
{
    private const string MESSAGE_KEY = "PageOutOfBounds";
    public PageOutOfBoundsException() : base(MESSAGE_KEY)
    {
    }
}

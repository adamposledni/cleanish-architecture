namespace Onion.Application.Services.Exceptions
{
    public class ItemNotFoundException : NotFoundException
    {
        private const string MESSAGE_KEY = "ItemNotFoundMessage";
        public ItemNotFoundException(int itemId) : base(itemId, MESSAGE_KEY)
        {
        }
    }
}

using Onion.Application.DataAccess.Exceptions.Common;
using System;

namespace Onion.Application.DataAccess.Exceptions.Item
{
    public class ItemNotFoundException : NotFoundException
    {
        private const string MESSAGE_KEY = "ItemNotFoundMessage";
        public ItemNotFoundException(Guid itemId) : base(itemId, MESSAGE_KEY)
        {
        }
    }
}

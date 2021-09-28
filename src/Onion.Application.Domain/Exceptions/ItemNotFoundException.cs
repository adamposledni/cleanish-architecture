using System;
using System.Collections.Generic;
using System.Text;

namespace Onion.Application.Domain.Exceptions
{
    public class ItemNotFoundException : NotFoundException
    {
        public ItemNotFoundException(int itemId) : base($"Item with ID {itemId} was not found.")
        {
        }
    }
}

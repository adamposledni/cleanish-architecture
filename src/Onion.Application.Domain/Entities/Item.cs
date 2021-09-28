using System;
using System.Collections.Generic;
using System.Text;

namespace Onion.Application.Domain.Entities
{
    public class Item : BaseEntity
    {
        public string Title { get; set; }
        public string Description { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace Onion.Application.Services.Models.Item
{
    public class ItemRes
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime Created { get; set; }
        public DateTime Updated { get; set; }
    }
}

using System;

namespace Onion.Application.Services.ItemManagement.Models
{
    public class ItemRes
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime Created { get; set; }
        public DateTime Updated { get; set; }
    }
}

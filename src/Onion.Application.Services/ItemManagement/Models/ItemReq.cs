using System.ComponentModel.DataAnnotations;

namespace Onion.Application.Services.ItemManagement.Models
{
    public class ItemReq
    {
        [Required]
        public string Title { get; set; }
        public string Description { get; set; }
    }
}

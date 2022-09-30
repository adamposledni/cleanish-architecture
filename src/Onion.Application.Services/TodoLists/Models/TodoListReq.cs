using System.ComponentModel.DataAnnotations;

namespace Onion.Application.Services.TodoLists.Models;

public class TodoListReq
{
    [Required]
    public string Title { get; set; }
}

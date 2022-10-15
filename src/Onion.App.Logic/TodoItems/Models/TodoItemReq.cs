using System.ComponentModel.DataAnnotations;

namespace Onion.App.Logic.TodoLists.Models;

public class TodoItemReq
{
    [Required]
    public string Title { get; set; }
}

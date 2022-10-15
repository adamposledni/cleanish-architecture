using System.ComponentModel.DataAnnotations;

namespace Onion.App.Logic.TodoLists.Models;

public class TodoListReq
{
    [Required]
    public string Title { get; set; }
}

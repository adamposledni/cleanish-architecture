using Onion.Application.DataAccess.Database.Entities.Fields;
using System.ComponentModel.DataAnnotations;

namespace Onion.Application.Services.TodoLists.Models;

public class TodoItemReq
{
    [Required]
    public string Title { get; set; }

    [Required]
    public Guid TodoListId { get; set; }
}

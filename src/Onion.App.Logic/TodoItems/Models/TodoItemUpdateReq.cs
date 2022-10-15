using Onion.App.Data.Database.Entities.Fields;
using System.ComponentModel.DataAnnotations;

namespace Onion.App.Logic.TodoItems.Models;

public class TodoItemUpdateReq
{
    [Required]
    public Guid Id { get; set; }

    [Required]
    public string Title { get; set; }

    [Required]
    public TodoItemState State { get; set; }
}

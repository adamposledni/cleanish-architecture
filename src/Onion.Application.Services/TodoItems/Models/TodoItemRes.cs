using Onion.Application.DataAccess.Database.Entities.Fields;

namespace Onion.Application.Services.TodoLists.Models;

public class TodoItemRes
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public TodoItemState State { get; set; }
}

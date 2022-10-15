using Onion.App.Data.Database.Entities.Fields;

namespace Onion.App.Data.Database.Entities;

public class TodoItem : BaseEntity
{
    public string Title { get; set; }
    public TodoItemState State { get; set; } = TodoItemState.New;
    public Guid TodoListId { get; set; }

    public TodoList TodoList { get; set; }
}

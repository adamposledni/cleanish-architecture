using Cleanish.App.Data.Database.Entities.Fields;

namespace Cleanish.App.Logic.TodoLists.Models;

public class TodoItemRes
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public TodoItemState State { get; set; }
}

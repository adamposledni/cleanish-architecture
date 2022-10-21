using Onion.App.Data.Database.Entities.Fields;

namespace Onion.App.Logic.TodoLists.Models;

public class TodoItemRes
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public TodoItemState State { get; set; }
    public byte[] Version { get; set; }
}

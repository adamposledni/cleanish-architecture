namespace Onion.Application.Services.TodoLists.Models;

public class TodoListRes
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public IEnumerable<TodoItemRes> TodoItems { get; set; }
}

namespace Onion.Application.DataAccess.Database.Entities;

public class TodoList : BaseEntity
{
    public string Title { get; set; }
    public Guid UserId { get; set; }

    public User User { get; set; }
    public IEnumerable<TodoItem> TodoItems { get; set; }
}

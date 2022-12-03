using Cleanish.App.Data.Database.Entities.Fields;

namespace Cleanish.App.Data.Database.Entities;

public class TodoItem : BaseEntity
{
    public string Title { get; set; }
    public TodoItemState State { get; set; } = TodoItemState.New;
    public Guid UserId { get; set; }

    public User User { get; set; }
}

using Onion.App.Data.Database.Entities;

namespace Onion.App.Data.Database.Repositories;

public interface ITodoItemRepository : IDatabaseRepository<TodoItem>
{
    Task<TodoItem> GetByIdAsync(Guid todoItemId);
    Task<IEnumerable<TodoItem>> ListAsync(Guid todoListId);
}

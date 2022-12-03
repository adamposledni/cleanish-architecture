using Cleanish.App.Data.Database.Entities;

namespace Cleanish.App.Data.Database.Repositories;

public interface ITodoItemRepository : IDatabaseRepository<TodoItem>
{
    Task<TodoItem> GetByIdAsync(Guid todoItemId);
    Task<IEnumerable<TodoItem>> ListByUserIdAsync(Guid userId);
}

using Onion.Application.DataAccess.Database.Entities;

namespace Onion.Application.DataAccess.Database.Repositories;

public interface ITodoItemRepository : IDatabaseRepository<TodoItem>
{

    Task<TodoItem> GetByIdAsync(Guid todoItemId);
    Task<IEnumerable<TodoItem>> ListAsync(Guid todoListId);
}

using Onion.App.Data.Database.Entities;

namespace Onion.App.Data.Database.Repositories;

public interface ITodoListRepository : IDatabaseRepository<TodoList>
{

    Task<TodoList> GetByIdAsync(Guid todoListId);
    Task<IEnumerable<TodoList>> ListAsync(Guid userId);
    Task<bool> AnyWithIdAndUserIdAsync(Guid todoListId, Guid userId);
}

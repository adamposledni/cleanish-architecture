using Onion.Application.DataAccess.Database.Entities;

namespace Onion.Application.DataAccess.Database.Repositories;

public interface ITodoListRepository : IDatabaseRepository<TodoList>
{

    Task<TodoList> GetByIdAsync(Guid todoListId);
    Task<IEnumerable<TodoList>> ListAsync();
    Task<bool> AnyWithIdAndUserIdAsync(Guid todoListId, Guid userId);
}

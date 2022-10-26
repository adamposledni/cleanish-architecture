using Microsoft.EntityFrameworkCore;
using Onion.App.Data.Cache;
using Onion.App.Data.Database.Entities;
using Onion.App.Data.Database.Repositories;

namespace Onion.Impl.App.Data.Database.Repositories;

internal class TodoItemRepository : DatabaseRepository<TodoItem>, ITodoItemRepository
{
    public TodoItemRepository(SqlDbContext dbContext, ICacheService<TodoItem> cacheService, CacheStrategy cacheStrategy)
        : base(dbContext, cacheService, cacheStrategy)
    { }

    public async Task<TodoItem> GetByIdAsync(Guid todoItemId)
    {
        return await ReadDataAsync(
            Specification()
            .SetFilter(ti => ti.Id == todoItemId)
            .Build(),
            q => q.SingleOrDefaultAsync()
        );
    }

    public async Task<IEnumerable<TodoItem>> ListAsync(Guid todoListId)
    {
        return await ReadDataAsync(
            Specification()
            .SetFilter(ti => ti.TodoListId == todoListId)
            .Build(),
            q => q.ToListAsync()
        );
    }
}

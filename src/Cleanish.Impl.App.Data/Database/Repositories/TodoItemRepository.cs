using Microsoft.EntityFrameworkCore;
using Cleanish.App.Data.Cache;
using Cleanish.App.Data.Database.Entities;
using Cleanish.App.Data.Database.Repositories;

namespace Cleanish.Impl.App.Data.Database.Repositories;

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

    public async Task<IEnumerable<TodoItem>> ListByUserIdAsync(Guid userId)
    {
        return await ReadDataAsync(
            Specification()
            .SetFilter(ti => ti.UserId == userId)
            .Build(),
            q => q.ToListAsync()
        );
    }
}

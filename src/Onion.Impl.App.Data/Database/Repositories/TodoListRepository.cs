using Microsoft.EntityFrameworkCore;
using Onion.App.Data.Cache;
using Onion.App.Data.Database.Entities;
using Onion.App.Data.Database.Repositories;

namespace Onion.Impl.App.Data.Database.Repositories;

internal class TodoListRepository : DatabaseRepository<TodoList>, ITodoListRepository
{
    public TodoListRepository(SqlDbContext dbContext, ICacheService cacheService, CacheStrategy cacheStrategy)
        : base(dbContext, cacheService, cacheStrategy)
    { }

    public async Task<bool> AnyWithIdAndUserIdAsync(Guid todoListId, Guid userId)
    {
        return await ReadDataAsync(
            Specification().SetFilter(tl => tl.UserId == userId).Build(),
            q => q.AnyAsync()
        );
    }

    public async Task<TodoList> GetByIdAsync(Guid todoListId)
    {
        return await ReadDataAsync(
            Specification()
            .SetFilter(tl => tl.Id == todoListId)
            .AddInclude(q => q.Include(tl => tl.TodoItems))
            .Build(),
            q => q.SingleOrDefaultAsync()
        );
    }

    public async Task<IEnumerable<TodoList>> ListAsync(Guid userId)
    {
        return await ReadDataAsync(
            Specification().SetFilter(tl => tl.UserId == userId).Build(),
            q => q.ToListAsync()
        );
    }
}
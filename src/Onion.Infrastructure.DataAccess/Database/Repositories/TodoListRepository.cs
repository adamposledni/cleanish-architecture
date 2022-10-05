using Microsoft.EntityFrameworkCore;
using Onion.Core.Helpers;
using Onion.Application.DataAccess.Database.Entities;
using Onion.Application.DataAccess.Database.Repositories;
using Onion.Core.Cache;

namespace Onion.Infrastructure.DataAccess.Database.Repositories;

public class TodoListRepository : DatabaseRepository<TodoList>, ITodoListRepository
{
    public TodoListRepository(SqlDbContext dbContext, ICacheService cacheService)
        : base(dbContext, cacheService)
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

    public async Task<IEnumerable<TodoList>> ListAsync()
    {
        return await ReadDataAsync(
            q => q.ToListAsync()
        );
    }
}
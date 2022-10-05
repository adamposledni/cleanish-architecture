using Microsoft.EntityFrameworkCore;
using Onion.Application.DataAccess.Database.Entities;
using Onion.Application.DataAccess.Database.Repositories;
using Onion.Core.Cache;

namespace Onion.Infrastructure.DataAccess.Database.Repositories;

public class TodoItemRepository : DatabaseRepository<TodoItem>, ITodoItemRepository
{
    public TodoItemRepository(SqlDbContext dbContext, ICacheService cacheService)
        : base(dbContext, cacheService)
    { }

    public async Task<TodoItem> GetByIdAsync(Guid todoItemId)
    {
        return await ReadDataAsync(
            Specification().SetFilter(ti => ti.Id == todoItemId).Build(),
            q => q.SingleOrDefaultAsync()
        );
    }

    public async Task<IEnumerable<TodoItem>> ListAsync(Guid todoListId)
    {
        return await ReadDataAsync(
            Specification().SetFilter(ti => ti.TodoListId == todoListId).Build(),
            q => q.ToListAsync()
        );
    }
}

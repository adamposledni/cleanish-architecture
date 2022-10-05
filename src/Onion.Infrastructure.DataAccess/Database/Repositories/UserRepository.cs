using Microsoft.EntityFrameworkCore;
using Onion.Core.Helpers;
using Onion.Application.DataAccess.Database.Entities;
using Onion.Application.DataAccess.Database.Repositories;
using Onion.Core.Cache;

namespace Onion.Infrastructure.DataAccess.Database.Repositories;

public class UserRepository : DatabaseRepository<User>, IUserRepository
{
    public UserRepository(SqlDbContext dbContext, ICacheService cacheService)
        : base(dbContext, cacheService)
    { }

    public async Task<User> GetByIdAsync(Guid userId)
    {
        return await ReadDataAsync(
            Specification().SetFilter(u => u.Id == userId).Build(),
            q => q.SingleOrDefaultAsync()
        );
    }

    public async Task<User> GetByEmailAsync(string email)
    {
        Guard.NotNullOrEmptyOrWhiteSpace(email, nameof(email));

        return await ReadDataAsync(
            Specification().SetFilter(u => u.Email == email).Build(),
            q => q.SingleOrDefaultAsync()
        );
    }

    public async Task<User> GetByGoogleIdAsync(string googleId)
    {
        Guard.NotNullOrEmptyOrWhiteSpace(googleId, nameof(googleId));

        return await ReadDataAsync(
            Specification().SetFilter(u => u.GoogleSubjectId == googleId).Build(),
            q => q.SingleOrDefaultAsync()
        );
    }

    public async Task<bool> AnyWithEmailAsync(string email)
    {
        Guard.NotNullOrEmptyOrWhiteSpace(email, nameof(email));

        return await ReadDataAsync(
            Specification().SetFilter(u => u.Email == email).Build(),
            q => q.AnyAsync()
        );
    }

    public async Task<IEnumerable<User>> ListAsync()
    {
        return await ReadDataAsync(
            q => q.ToListAsync()
        );
    }

    public async Task<User> FooAsync()
    {
        return await ReadDataAsync(
            Specification()
            .SetFilter(u => u.Email == "hroudaadam@gmail.com")
            .AddInclude(q => q.Include(u => u.TodoLists).ThenInclude(tl => tl.TodoItems))
            .Build(),
            q => q.SingleOrDefaultAsync()
        );
    }
}
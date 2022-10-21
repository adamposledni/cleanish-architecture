using Microsoft.EntityFrameworkCore;
using Onion.App.Data.Cache;
using Onion.App.Data.Database.Entities;
using Onion.App.Data.Database.Repositories;
using Onion.Shared.Helpers;

namespace Onion.Impl.App.Data.Database.Repositories;

public class UserRepository : DatabaseRepository<User>, IUserRepository
{
    public UserRepository(SqlDbContext dbContext, ICacheService cacheService, CacheStrategy cacheStrategy)
        : base(dbContext, cacheService, cacheStrategy)
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
            Specification().Build(),
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
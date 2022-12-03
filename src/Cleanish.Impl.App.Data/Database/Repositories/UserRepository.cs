using Microsoft.EntityFrameworkCore;
using Cleanish.App.Data.Cache;
using Cleanish.App.Data.Database.Entities;
using Cleanish.App.Data.Database.Repositories;
using Cleanish.Shared.Helpers;

namespace Cleanish.Impl.App.Data.Database.Repositories;

internal class UserRepository : DatabaseRepository<User>, IUserRepository
{
    public UserRepository(SqlDbContext dbContext, ICacheService<User> cacheService, CacheStrategy cacheStrategy)
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
            Specification()
            .SetFilter(u => u.Email == email).Build(),
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
}
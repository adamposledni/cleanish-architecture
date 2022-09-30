using Microsoft.EntityFrameworkCore;
using Onion.Core.Helpers;
using Onion.Application.DataAccess.Database.Entities;
using Onion.Application.DataAccess.Database.Repositories;
using Onion.Core.Cache;
using Onion.Infrastructure.DataAccess.Database.Specifications;

namespace Onion.Infrastructure.DataAccess.Database.Repositories;

public class UserRepository : DatabaseRepository<User>, IUserRepository
{
    public UserRepository(SqlDbContext dbContext, ICacheService cacheService, CacheStrategy cacheStrategy)
        : base(dbContext, cacheService, cacheStrategy)
    { }

    public async Task<User> GetByEmailAsync(string email)
    {
        Guard.NotNullOrEmptyOrWhiteSpace(email, nameof(email));

        return await ReadDataAsync(
            Specification().SetFilter(u => u.Email == email)
            .AddInclude(q => q.Include(u => u.TodoLists).ThenInclude(tl => tl.TodoItems)).Build(),
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

    public async Task<bool> EmailAlreadyExistsAsync(string email)
    {
        Guard.NotNullOrEmptyOrWhiteSpace(email, nameof(email));

        return await ReadDataAsync(
            Specification().SetFilter(u => u.Email == email).Build(),
            q => q.AnyAsync()
        );
    }
}
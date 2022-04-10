using Microsoft.EntityFrameworkCore;
using Onion.Core.Helpers;
using Onion.Application.DataAccess.Database.Entities;
using Onion.Application.DataAccess.Database.Repositories;
using Onion.Core.Cache;

namespace Onion.Infrastructure.DataAccess.Database.Repositories;

public class UserRepository : CachedDatabaseRepository<User>, IUserRepository
{
    public UserRepository(SqlDbContext dbContext, ICacheService cacheService)
        : base(dbContext, cacheService)
    { }

    public async Task<User> GetByEmailAsync(string email)
    {
        Guard.NotNullOrEmptyOrWhiteSpace(email, nameof(email));

        return await _dbSet.SingleOrDefaultAsync(u => u.Email == email);
    }

    public async Task<User> GetByGoogleIdAsync(string googleId)
    {
        Guard.NotNullOrEmptyOrWhiteSpace(googleId, nameof(googleId));

        return await _dbSet.SingleOrDefaultAsync(u => u.GoogleSubjectId == googleId);
    }

    public async Task<bool> EmailAlreadyExistsAsync(string email)
    {
        Guard.NotNullOrEmptyOrWhiteSpace(email, nameof(email));

        return await _dbSet.AnyAsync(u => u.Email == email);
    }
}
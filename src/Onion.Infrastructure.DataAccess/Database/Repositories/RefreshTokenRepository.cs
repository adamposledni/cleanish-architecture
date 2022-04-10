using Microsoft.EntityFrameworkCore;
using Onion.Core.Helpers;
using Onion.Application.DataAccess.Database.Repositories;
using Onion.Application.DataAccess.Database.Entities;
using Onion.Core.Cache;

namespace Onion.Infrastructure.DataAccess.Database.Repositories;

public class RefreshTokenRepository : CachedDatabaseRepository<RefreshToken>, IRefreshTokenRepository
{
    public RefreshTokenRepository(SqlDbContext dbContext, ICacheService cacheService) 
        : base(dbContext, cacheService)
    { }

    public async Task<RefreshToken> GetByTokenAndUserIdAsync(string token, Guid userId)
    {
        Guard.NotNullOrEmptyOrWhiteSpace(token, nameof(token));

        return await _dbSet.SingleOrDefaultAsync(rt => rt.UserId == userId && rt.Token == token);
    }

    public async Task<RefreshToken> GetByTokenAsync(string token)
    {
        Guard.NotNullOrEmptyOrWhiteSpace(token, nameof(token));

        return await _dbSet.Where(rt => rt.Token == token)
            .Include(rt => rt.User)
            .SingleOrDefaultAsync();
    }
}

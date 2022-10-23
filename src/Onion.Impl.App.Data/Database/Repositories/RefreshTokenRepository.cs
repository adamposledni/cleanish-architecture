using Microsoft.EntityFrameworkCore;
using Onion.App.Data.Cache;
using Onion.App.Data.Database.Entities;
using Onion.App.Data.Database.Repositories;

namespace Onion.Impl.App.Data.Database.Repositories;

public class RefreshTokenRepository : DatabaseRepository<RefreshToken>, IRefreshTokenRepository
{
    public RefreshTokenRepository(SqlDbContext dbContext, ICacheService<RefreshToken> cacheService, CacheStrategy cacheStrategy)
    : base(dbContext, cacheService, cacheStrategy)
    { }

    public async Task<RefreshToken> GetByTokenAndUserIdAsync(string token, Guid userId)
    {
        return await ReadDataAsync(
            Specification().SetFilter(rt => rt.Token == token && rt.UserId == userId).Build(),
            q => q.SingleOrDefaultAsync()
        );
    }

    public async Task<RefreshToken> GetByTokenAsync(string token)
    {
        return await ReadDataAsync(
            Specification()
            .SetFilter(rt => rt.Token == token)
            .AddInclude(q => q.Include(rt => rt.User))
            .Build(),
            q => q.SingleOrDefaultAsync()
        );
    }
}

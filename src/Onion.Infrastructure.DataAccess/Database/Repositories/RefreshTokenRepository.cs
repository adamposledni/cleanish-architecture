using Microsoft.EntityFrameworkCore;
using Onion.Application.DataAccess.Database.Entities;
using Onion.Application.DataAccess.Database.Repositories;
using Onion.Core.Cache;

namespace Onion.Infrastructure.DataAccess.Database.Repositories;

public class RefreshTokenRepository : DatabaseRepository<RefreshToken>, IRefreshTokenRepository
{
    public RefreshTokenRepository(SqlDbContext dbContext, ICacheService cacheService)
    : base(dbContext, cacheService)
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
            Specification().SetFilter(rt => rt.Token == token).Build(),
            q => q.SingleOrDefaultAsync()
        );
    }
}

using Microsoft.EntityFrameworkCore;
using Onion.Application.DataAccess.Entities;
using Onion.Application.DataAccess.Repositories;
using Onion.Core.Helpers;

namespace Onion.Infrastructure.DataAccess.Sql.Repositories;

public class RefreshTokenRepository : GenericRepository<RefreshToken>, IRefreshTokenRepository
{
    public RefreshTokenRepository(SqlDbContext dbContext) : base(dbContext)
    { }

    public async Task<RefreshToken> GetByTokenAndUserIdAsync(string token, Guid userId)
    {
        Guard.NotNullOrEmptyOrWhiteSpace(token, nameof(token));

        return await _dbSet.FirstOrDefaultAsync(rt => rt.UserId == userId && rt.Token == token);
    }

    public async Task<RefreshToken> GetByTokenAsync(string token)
    {
        Guard.NotNullOrEmptyOrWhiteSpace(token, nameof(token));

        return await _dbSet
            .Where(rt => rt.Token == token)
            .Include(rt => rt.User)
            .FirstOrDefaultAsync();
    }
}

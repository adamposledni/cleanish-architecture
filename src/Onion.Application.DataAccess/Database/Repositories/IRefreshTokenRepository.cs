using Onion.Application.DataAccess.Database.Entities;

namespace Onion.Application.DataAccess.Database.Repositories;

public interface IRefreshTokenRepository : IDatabaseRepository<RefreshToken>
{
    Task<RefreshToken> GetByTokenAsync(string token);
    Task<RefreshToken> GetByTokenAndUserIdAsync(string token, Guid userId);
}

using Onion.App.Data.Database.Entities;

namespace Onion.App.Data.Database.Repositories;

public interface IRefreshTokenRepository : IDatabaseRepository<RefreshToken>
{
    Task<RefreshToken> GetByTokenAsync(string token);
    Task<RefreshToken> GetByTokenAndUserIdAsync(string token, Guid userId);
}

using Onion.Application.DataAccess.Entities;
using System.Threading.Tasks;

namespace Onion.Application.DataAccess.Repositories;

public interface IRefreshTokenRepository : IGenericRepository<RefreshToken>
{
    Task<RefreshToken> GetByTokenAndUserIdAsync(string token, Guid userId);
    Task<RefreshToken> GetByTokenAsync(string token);
}

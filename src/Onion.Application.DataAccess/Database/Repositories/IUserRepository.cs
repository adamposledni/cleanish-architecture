using Onion.Application.DataAccess.Database.Entities;

namespace Onion.Application.DataAccess.Database.Repositories;

public interface IUserRepository : IDatabaseRepository<User>
{
    public Task<User> GetByEmailAsync(string email);
    public Task<User> GetByGoogleIdAsync(string googleId);
    Task<bool> EmailAlreadyExistsAsync(string email);
}

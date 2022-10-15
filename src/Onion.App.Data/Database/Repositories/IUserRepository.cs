using Onion.App.Data.Database.Entities;

namespace Onion.App.Data.Database.Repositories;

public interface IUserRepository : IDatabaseRepository<User>
{
    Task<User> GetByIdAsync(Guid userId);
    Task<User> GetByEmailAsync(string email);
    Task<User> GetByGoogleIdAsync(string googleId);
    Task<bool> AnyWithEmailAsync(string email);
    Task<IEnumerable<User>> ListAsync();
    Task<User> FooAsync();
}

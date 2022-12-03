using Cleanish.App.Data.Database.Entities;

namespace Cleanish.App.Data.Database.Repositories;

public interface IUserRepository : IDatabaseRepository<User>
{
    Task<User> GetByIdAsync(Guid userId);
    Task<User> GetByEmailAsync(string email);
    Task<bool> AnyWithEmailAsync(string email);
    Task<IEnumerable<User>> ListAsync();
}

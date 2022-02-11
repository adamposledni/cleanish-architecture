using Onion.Application.DataAccess.Entities;
using System.Threading.Tasks;

namespace Onion.Application.DataAccess.Repositories;

public interface IUserRepository : IGenericRepository<User>
{
    public Task<User> GetByEmailAsync(string email);
    public Task<User> GetByGoogleIdAsync(string googleId);
    Task<bool> EmailAlreadyExistsAsync(string email);
}

using Microsoft.EntityFrameworkCore;
using Onion.Application.DataAccess.Entities;
using Onion.Application.DataAccess.Repositories;
using Onion.Core.Helpers;

namespace Onion.Infrastructure.DataAccess.Sql.Repositories;

public class UserRepository : GenericRepository<User>, IUserRepository
{
    public UserRepository(SqlDbContext dbContext)
        : base(dbContext)
    { }

    public async Task<User> GetByEmailAsync(string email)
    {
        Guard.NotNullOrEmptyOrWhiteSpace(email, nameof(email));

        return await _dbSet.SingleOrDefaultAsync(u => u.Email == email);
    }

    public async Task<User> GetByGoogleIdAsync(string googleId)
    {
        Guard.NotNullOrEmptyOrWhiteSpace(googleId, nameof(googleId));

        return await _dbSet.SingleOrDefaultAsync(e => e.GoogleSubjectId == googleId);
    }

    public async Task<bool> EmailAlreadyExistsAsync(string email)
    {
        Guard.NotNullOrEmptyOrWhiteSpace(email, nameof(email));

        return await _dbSet.AnyAsync(u => u.Email == email);
    }
}
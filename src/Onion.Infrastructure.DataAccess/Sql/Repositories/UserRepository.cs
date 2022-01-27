using Microsoft.EntityFrameworkCore;
using Onion.Application.DataAccess.Entities;
using Onion.Application.DataAccess.Repositories;

namespace Onion.Infrastructure.DataAccess.Sql.Repositories;

public class UserRepository : GenericRepository<User>, IUserRepository
{
    public UserRepository(SqlDbContext dbContext)
        : base(dbContext)
    { }

    public async Task<User> GetByEmailAsync(string email)
    {
        return await _dbSet.FirstOrDefaultAsync(u => u.Email == email);
    }

    public async Task<User> GetByGoogleIdAsync(string googleId)
    {
        return await _dbSet.FirstOrDefaultAsync(e => e.GoogleSubjectId == googleId);
    }
}

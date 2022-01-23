using Onion.Application.DataAccess.Entities;
using Onion.Application.DataAccess.Repositories;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace Onion.Infrastucture.DataAccess.Sql.Repositories
{
    public class UserRepository : GenericRepository<User>, IUserRepository
    {
        public UserRepository(SqlDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<User> GetByEmailAsync(string email)
        {
            return await _dbSet.FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<User> GetByGoogleIdAsync(string googleId)
        {
            return await _dbSet.FirstOrDefaultAsync(e => e.GoogleSubjectId == googleId);
        }
    }
}

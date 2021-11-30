using MongoDB.Driver;
using Onion.Application.DataAccess.Entities;
using Onion.Application.DataAccess.Repositories;
using Onion.Core.Clock;
using System.Threading.Tasks;

namespace Onion.Infrastucture.DataAccess.MongoDb.Repositories
{
    public class UserRepository : GenericRepository<User>, IUserRepository
    {
        public UserRepository(IMongoDbContext mongoDbContext) 
            : base(mongoDbContext)
        {
        }

        public async Task<User> GetByEmailAsync(string email)
        {
            var filter = Builders<User>.Filter.Eq(u => u.Email, email);
            return await (await _dbSet.FindAsync(filter)).FirstOrDefaultAsync();
        }
    }
}

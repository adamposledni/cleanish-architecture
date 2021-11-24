using MongoDB.Driver;
using Onion.Application.DataAccess.Entities;
using Onion.Application.DataAccess.Repositories;

namespace Onion.Infrastucture.DataAccess.MongoDb.Repositories
{
    public class UserRepository : GenericRepository<User>, IUserRepository
    {
        public UserRepository(IMongoDbContext mongoDbContext) : base(mongoDbContext)
        {
        }
    }
}

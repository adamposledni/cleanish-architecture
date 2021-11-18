using MongoDB.Driver;
using Onion.Application.DataAccess.Database.Entities;
using Onion.Application.DataAccess.Database.Repositories;

namespace Onion.Infrastucture.DataAccess.NoSql.Repositories
{
    public class ItemRepository : GenericRepository<Item>, IItemRepository
    {
        public ItemRepository(MongoDbContext mongoDbContext) : base(mongoDbContext)
        {
        }
    }
}

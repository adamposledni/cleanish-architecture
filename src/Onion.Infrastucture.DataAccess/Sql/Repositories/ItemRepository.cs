using Onion.Application.DataAccess.Database.Entities;
using Onion.Application.DataAccess.Database.Repositories;

namespace Onion.Infrastucture.DataAccess.Sql.Repositories
{
    public class ItemRepository : GenericRepository<Item>, IItemRepository
    {
        public ItemRepository(SqlDbContext dbContext) : base(dbContext)
        {
        }
    }
}

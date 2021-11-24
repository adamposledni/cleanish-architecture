using Onion.Application.DataAccess.Entities;
using Onion.Application.DataAccess.Repositories;

namespace Onion.Infrastucture.DataAccess.Sql.Repositories
{
    public class ItemRepository : GenericRepository<Item>, IItemRepository
    {
        public ItemRepository(SqlDbContext dbContext, bool isTransactional) : base(dbContext, isTransactional)
        {
        }
    }
}

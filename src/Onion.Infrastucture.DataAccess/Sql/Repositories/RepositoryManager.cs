using Onion.Application.DataAccess.Database.Repositories;
using System;
using System.Threading.Tasks;

namespace Onion.Infrastucture.DataAccess.Sql.Repositories
{
    public class RepositoryManager : IRepositoryManager
    {
        private readonly SqlDbContext _dbContext;
        private Lazy<IItemRepository> _itemRepository;

        public RepositoryManager(SqlDbContext dbContext)
        {
            _dbContext = dbContext;
            _itemRepository = new Lazy<IItemRepository>(() => new ItemRepository(_dbContext));
        }

        public IItemRepository ItemRepository => _itemRepository.Value;

        public async Task SaveChangesAsync()
        {
            await _dbContext.SaveChangesAsync();
        }
    }
}

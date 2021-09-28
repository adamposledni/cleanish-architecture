using Onion.Application.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Onion.Infrastucture.Persistence.Repositories
{
    public class RepositoryManager : IRepositoryManager
    {
        private OnionDbContext _dbContext;

        public RepositoryManager(OnionDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        private IItemRepository _itemRepository;
        public IItemRepository ItemRepository
        {
            get
            {
                _itemRepository ??= new ItemRepository(_dbContext);
                return _itemRepository;
            }
        }

        public async Task SaveChangesAsync()
        {
            await _dbContext.SaveChangesAsync();
        }
    }
}

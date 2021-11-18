using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Onion.Application.DataAccess.Database.Repositories;
using Onion.Infrastucture.DataAccess.NoSql.Configuration;
using System;
using System.Threading.Tasks;

namespace Onion.Infrastucture.DataAccess.NoSql.Repositories
{
    public class RepositoryManager : IRepositoryManager
    {
        private readonly MongoDbContext _mongoDbContext;
        private Lazy<IItemRepository> _itemRepository;

        public RepositoryManager(MongoDbContext mongoDbContext)
        {            
            _mongoDbContext = mongoDbContext;
            _itemRepository = new Lazy<IItemRepository>(() => new ItemRepository(_mongoDbContext));
        }

        public IItemRepository ItemRepository => _itemRepository.Value;

        public async Task SaveChangesAsync()
        {
            await _mongoDbContext.SaveChangesAsync();   
        }
    }
}

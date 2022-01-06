using Onion.Application.DataAccess.Repositories;
using Onion.Infrastucture.DataAccess.MongoDb;
using Onion.Infrastucture.DataAccess.MongoDb.Repositories;
using Onion.Infrastucture.DataAccess.Sql;
using Onion.Infrastucture.DataAccess.Sql.Repositories;
using System;

namespace Onion.Infrastucture.DataAccess
{
    public class RepositoryManager : IRepositoryManager
    {
        protected readonly IMongoDbContext _mongoDbContext;
        protected readonly SqlDbContext _sqlDbContext;
        protected bool _isTransactional = false;

        private readonly Lazy<IItemRepository> _itemRepository;
        public IItemRepository ItemRepository => _itemRepository.Value;

        private readonly Lazy<IUserRepository> _userRepository;
        public IUserRepository UserRepository => _userRepository.Value;

        public RepositoryManager(IMongoDbContext mongoDbContext, SqlDbContext sqlDbContext)
        {
            _mongoDbContext = mongoDbContext;
            _sqlDbContext = sqlDbContext;

            _itemRepository = new Lazy<IItemRepository>(() => new ItemRepository(_sqlDbContext, _isTransactional));
            _userRepository = new Lazy<IUserRepository>(() => new UserRepository(_mongoDbContext));
        }
    }
}

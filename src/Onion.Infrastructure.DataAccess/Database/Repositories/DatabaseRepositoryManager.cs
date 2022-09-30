using Onion.Application.DataAccess.Database.Entities;
using Onion.Application.DataAccess.Database.Repositories;
using Onion.Core.Cache;

namespace Onion.Infrastructure.DataAccess.Database.Repositories;

public class DatabaseRepositoryManager : IDatabaseRepositoryManager
{
    private readonly SqlDbContext _dbContext;
    private readonly ICacheService _cacheService;

    public DatabaseRepositoryManager(SqlDbContext dbContext, ICacheService cacheService)
    {
        _dbContext = dbContext;
        _cacheService = cacheService;
    }

    public IDatabaseRepository<T> GetDatabaseRepository<T>(CacheStrategy cacheStrategy) where T : BaseEntity
    {
        // TODO: reuse repositories
        return new DatabaseRepository<T>(_dbContext, _cacheService, cacheStrategy);
    }
}


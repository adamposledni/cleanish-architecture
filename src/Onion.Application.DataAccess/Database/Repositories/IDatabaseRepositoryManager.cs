using Onion.Application.DataAccess.Database.Entities;
using Onion.Core.Cache;

namespace Onion.Application.DataAccess.Database.Repositories;

public interface IDatabaseRepositoryManager
{
    public TRepository GetRepository<TRepository, TEntity>(CacheStrategy cacheStrategy)
        where TRepository : IDatabaseRepository<TEntity> where TEntity : BaseEntity;
}

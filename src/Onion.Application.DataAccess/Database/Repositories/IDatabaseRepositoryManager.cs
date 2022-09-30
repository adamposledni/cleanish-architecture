using Onion.Application.DataAccess.Database.Entities;
using Onion.Core.Cache;

namespace Onion.Application.DataAccess.Database.Repositories;

public interface IDatabaseRepositoryManager
{
    IDatabaseRepository<T> GetDatabaseRepository<T>(CacheStrategy cacheStrategy) where T: BaseEntity;
}

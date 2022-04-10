using Onion.Application.DataAccess.Database.Entities;
using Onion.Core.Pagination;

namespace Onion.Application.DataAccess.Database.Repositories;

public interface ICachedDatabaseRepository<T> : IDatabaseRepository<T> where T : BaseEntity
{
    Task<IEnumerable<T>> CachedListAsync();
    Task<PaginableList<T>> CachedPaginateAsync(int pageSize, int page);
    Task<T> CachedGetByIdAsync(Guid entityId);
    Task<int> CachedCountAsync();
}

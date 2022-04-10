using Onion.Application.DataAccess.Database.Entities;
using Onion.Core.Pagination;

namespace Onion.Application.DataAccess.Database.Repositories;

public interface IDatabaseRepository<T> where T : BaseEntity
{
    Task<IEnumerable<T>> ListAsync();
    Task<PaginableList<T>> PaginateAsync(int pageSize, int page);
    Task<T> GetByIdAsync(Guid entityId);
    Task<T> CreateAsync(T newEntity);
    Task<T> DeleteAsync(T entityToDelete);
    Task<T> UpdateAsync(T updatedEntity);
    Task<int> CountAsync();
}

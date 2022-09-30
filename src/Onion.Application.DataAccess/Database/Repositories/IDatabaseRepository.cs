using Onion.Application.DataAccess.Database.Entities;
using Onion.Application.DataAccess.Specifications;
using Onion.Core.Pagination;

namespace Onion.Application.DataAccess.Database.Repositories;

public interface IDatabaseRepository<T> where T : BaseEntity
{
    Task<T> CreateAsync(T newEntity);
    Task<T> DeleteAsync(T entityToDelete);
    Task<T> UpdateAsync(T updatedEntity);
}

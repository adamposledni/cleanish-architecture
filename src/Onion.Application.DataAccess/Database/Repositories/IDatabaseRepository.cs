using Onion.Application.DataAccess.Database.Entities;
using Onion.Core.Cache;

namespace Onion.Application.DataAccess.Database.Repositories;

public interface IDatabaseRepository<T> where T : BaseEntity
{
    CacheStrategy CacheStrategy { get; set; }

    Task<T> CreateAsync(T newEntity);
    Task<T> DeleteAsync(T entityToDelete);
    Task<T> UpdateAsync(T updatedEntity);
}

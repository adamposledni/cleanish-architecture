using Onion.Application.DataAccess.Database.Entities;
using Onion.Core.Cache;

namespace Onion.Application.DataAccess.Database.Repositories;

public interface IDatabaseRepository<T> where T : BaseEntity
{
    CacheStrategy CacheStrategy { get; set; }

    Task<T> CreateAsync(T newEntity, bool commitAfter = true);
    Task<T> DeleteAsync(T entityToDelete, bool commitAfter = true);
    Task<T> UpdateAsync(T updatedEntity, bool commitAfter = true);
}

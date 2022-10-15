using Onion.App.Data.Cache;
using Onion.App.Data.Database.Entities;

namespace Onion.App.Data.Database.Repositories;

public interface IDatabaseRepository<T> : ICachable where T : BaseEntity
{
    Task<T> CreateAsync(T newEntity, bool commitAfter = true);
    Task<T> DeleteAsync(T entityToDelete, bool commitAfter = true);
    Task<T> UpdateAsync(T updatedEntity, bool commitAfter = true);
}

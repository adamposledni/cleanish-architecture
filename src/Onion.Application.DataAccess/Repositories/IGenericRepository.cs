using Onion.Application.DataAccess.Entities;
using Onion.Core.Structures;
using System.Threading.Tasks;

namespace Onion.Application.DataAccess.Repositories;

public interface IGenericRepository<T> where T : BaseEntity
{
    Task<IEnumerable<T>> ListAsync();
    Task<PaginableList<T>> PaginateAsync(int pageSize, int page);
    Task<T> GetByIdAsync(Guid entityId);
    Task<T> CreateAsync(T newEntity);
    Task<T> DeleteAsync(T entityToDelete);
    Task<T> UpdateAsync(T updatedEntity);
    Task<int> CountAsync();
}

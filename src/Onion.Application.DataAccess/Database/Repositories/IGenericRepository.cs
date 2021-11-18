using Onion.Application.DataAccess.Database.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Onion.Application.DataAccess.Database.Repositories
{
    public interface IGenericRepository<T> where T : DatabaseEntity
    {
        Task<IEnumerable<T>> ListAsync();
        Task<T> GetByIdAsync(int entityId);
        Task<T> CreateAsync(T newEntity, bool isTransactional = false);
        Task<T> DeleteAsync(T entityToDelete, bool isTransactional = false);
        Task<T> UpdateAsync(T updatedEntity, bool isTransactional = false);
    }
}

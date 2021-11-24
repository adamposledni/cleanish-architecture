using Onion.Application.DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Onion.Application.DataAccess.Repositories
{
    public interface IGenericRepository<T> where T : BaseEntity
    {
        Task<IList<T>> ListAsync();
        Task<T> GetByIdAsync(Guid entityId);
        Task<T> CreateAsync(T newEntity);
        Task<T> DeleteAsync(T entityToDelete);
        Task<T> UpdateAsync(T updatedEntity);
    }
}

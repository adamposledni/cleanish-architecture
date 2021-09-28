using Onion.Application.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Onion.Application.Domain.Repositories
{
    public interface IGenericRepository<T> where T: BaseEntity
    {
        Task<IEnumerable<T>> ListAsync();
        Task<T> GetByIdAsync(int entityId);
        T Create(T newEntity);
        T Delete(T entityToDelete);
    }
}

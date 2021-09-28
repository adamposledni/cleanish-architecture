using Microsoft.EntityFrameworkCore;
using Onion.Application.Domain.Entities;
using Onion.Application.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Onion.Infrastucture.Persistence.Repositories
{
    public abstract class GenericRepository<T> : IGenericRepository<T> where T: BaseEntity
    {
        protected OnionDbContext _dbContext;

        public GenericRepository(OnionDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public T Create(T newEntity)
        {
            return _dbContext.Set<T>().Add(newEntity).Entity;
        }

        public T Delete(T entityToDelete)
        {
            return _dbContext.Set<T>().Remove(entityToDelete).Entity;
        }

        public async Task<T> GetByIdAsync(int entityId)
        {
            return await _dbContext.Set<T>().FirstOrDefaultAsync(e => e.Id == entityId);
        }

        public async Task<IEnumerable<T>> ListAsync()
        {
            return await _dbContext.Set<T>().ToListAsync();
        }
    }
}

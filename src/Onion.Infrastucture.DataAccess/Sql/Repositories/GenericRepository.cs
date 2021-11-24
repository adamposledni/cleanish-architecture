using Microsoft.EntityFrameworkCore;
using Onion.Application.DataAccess.Entities;
using Onion.Application.DataAccess.Repositories;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Onion.Infrastucture.DataAccess.Sql.Repositories
{
    public abstract class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity
    {
        protected readonly SqlDbContext _dbContext;
        protected readonly DbSet<T> _dbSet;
        protected readonly bool _isTransactional;

        public GenericRepository(SqlDbContext dbContext, bool isTransactional)
        {
            _dbContext = dbContext;
            _dbSet = _dbContext.Set<T>();
            _isTransactional = isTransactional;
        }

        public async Task<T> CreateAsync(T newEntity)
        {
            T createdEntity = _dbSet.Add(newEntity).Entity;
            await TrySaveChangesAsync();
            return createdEntity;
        }

        public async Task<T> UpdateAsync(T updatedEntity)
        {
            await TrySaveChangesAsync();
            return updatedEntity;
        }

        public async Task<T> DeleteAsync(T entityToDelete)
        {
            T deletedEntity = _dbSet.Remove(entityToDelete).Entity;
            await TrySaveChangesAsync();
            return deletedEntity;
        }

        public async Task<T> GetByIdAsync(Guid entityId)
        {
            return await _dbSet.FirstOrDefaultAsync(e => e.Id == entityId);
        }

        public async Task<IList<T>> ListAsync()
        {
            return await _dbSet.ToListAsync();
        }

        protected async Task TrySaveChangesAsync()
        {
            if (!_isTransactional)
            {
                await _dbContext.SaveChangesAsync();
            }
        }
    }
}

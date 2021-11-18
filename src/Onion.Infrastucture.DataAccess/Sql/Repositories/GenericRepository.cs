using Microsoft.EntityFrameworkCore;
using Onion.Application.DataAccess.Database.Entities;
using Onion.Application.DataAccess.Database.Repositories;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Onion.Infrastucture.DataAccess.Sql.Repositories
{
    public abstract class GenericRepository<T> : IGenericRepository<T> where T : DatabaseEntity
    {
        protected readonly SqlDbContext _dbContext;
        protected readonly DbSet<T> _dbSet;

        public GenericRepository(SqlDbContext dbContext)
        {
            _dbContext = dbContext;
            _dbSet = _dbContext.Set<T>();
        }

        public async Task<T> CreateAsync(T newEntity, bool isTransactional = false)
        {
            T createdEntity = _dbSet.Add(newEntity).Entity;
            await TrySaveChangesAsync(isTransactional);
            return createdEntity;
        }

        public async Task<T> UpdateAsync(T updatedEntity, bool isTransactional = false)
        {
            await TrySaveChangesAsync(isTransactional);
            return updatedEntity;
        }

        public async Task<T> DeleteAsync(T entityToDelete, bool isTransactional = false)
        {
            T deletedEntity = _dbSet.Remove(entityToDelete).Entity;
            await TrySaveChangesAsync(isTransactional);
            return deletedEntity;
        }

        public async Task<T> GetByIdAsync(int entityId)
        {
            return await _dbSet.FirstOrDefaultAsync(e => e.Id == entityId);
        }

        public async Task<IEnumerable<T>> ListAsync()
        {
            return await _dbSet.ToListAsync();
        }

        protected async Task TrySaveChangesAsync(bool isTransactional)
        {
            if (!isTransactional)
            {
                await _dbContext.SaveChangesAsync();
            }
        }
    }
}

using Microsoft.EntityFrameworkCore;
using Onion.Application.DataAccess.Entities;
using Onion.Application.DataAccess.Exceptions.Common;
using Onion.Application.DataAccess.Repositories;
using Onion.Core.Structures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Onion.Infrastucture.DataAccess.Sql.Repositories
{
    public abstract class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity
    {
        protected readonly SqlDbContext _dbContext;
        protected readonly DbSet<T> _dbSet;

        public GenericRepository(SqlDbContext dbContext)
        {
            _dbContext = dbContext;
            _dbSet = _dbContext.Set<T>();
        }

        public async Task<T> CreateAsync(T newEntity)
        {
            T createdEntity = _dbSet.Add(newEntity).Entity;
            await _dbContext.SaveChangesAsync();
            return createdEntity;
        }

        public async Task<T> UpdateAsync(T updatedEntity)
        {
            await _dbContext.SaveChangesAsync();
            return updatedEntity;
        }

        public async Task<T> DeleteAsync(T entityToDelete)
        {
            T deletedEntity = _dbSet.Remove(entityToDelete).Entity;
            await _dbContext.SaveChangesAsync();
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

        public async Task<int> CountAsync()
        {
            return await _dbSet.CountAsync();
        }

        public async Task<PaginableList<T>> PaginateAsync(int pageSize, int page)
        {
            return await PaginateAsync(pageSize, page, e => true);
        }

        protected async Task<PaginableList<T>> PaginateAsync(int pageSize, int page, Func<T, bool> filter)
        {
            int countOfEntities = await CountAsync();
            int countOfPages = (int)Math.Ceiling((double)countOfEntities / pageSize);
            if (page > countOfPages) throw new PageOutOfBoundsException();

            var entities = await _dbSet
                .Where(e => filter(e))
                .Skip(pageSize * (page - 1)).Take(pageSize)
                .ToListAsync();

            return new PaginableList<T>(entities, countOfEntities, pageSize, page, countOfPages);
        }
    }
}

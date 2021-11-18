using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;
using Onion.Application.DataAccess.Database.Entities;
using Onion.Application.DataAccess.Database.Repositories;
using Onion.Infrastucture.DataAccess.NoSql.Configuration;
using Onion.Infrastucture.DataAccess.Sql;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Onion.Infrastucture.DataAccess.NoSql.Repositories
{
    public abstract class GenericRepository<T> : IGenericRepository<T> where T : DatabaseEntity
    {
        private readonly MongoDbContext _mongoDbContext;
        protected readonly IMongoCollection<T> _dbSet;

        public GenericRepository(MongoDbContext mongoDbContext)
        {
            _mongoDbContext = mongoDbContext;
            _dbSet = _mongoDbContext.Collection<T>();
        }

        public async Task<T> CreateAsync(T newEntity, bool isTransactional = false)
        {
            _mongoDbContext.AddCommand(() => _dbSet.InsertOneAsync(newEntity));

            await TrySaveChangesAsync(isTransactional);
            return newEntity;
        }

        public async Task<T> UpdateAsync(T updatedEntity, bool isTransactional = false)
        {
            var filter = Builders<T>.Filter.Eq(e => e.Id, updatedEntity.Id);
            _mongoDbContext.AddCommand(() => _dbSet.ReplaceOneAsync(filter, updatedEntity));

            await TrySaveChangesAsync(isTransactional);
            return updatedEntity;
        }

        public async Task<T> DeleteAsync(T entityToDelete, bool isTransactional = false)
        {
            var filter = Builders<T>.Filter.Eq(e => e.Id, entityToDelete.Id);
            _mongoDbContext.AddCommand(() => _dbSet.DeleteOneAsync(filter));

            await TrySaveChangesAsync(isTransactional);
            return entityToDelete;
        }

        public async Task<T> GetByIdAsync(int entityId)
        {
            var filter = Builders<T>.Filter.Eq(e => e.Id, entityId);
            return await (await _dbSet.FindAsync(filter)).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<T>> ListAsync()
        {
            var filter = Builders<T>.Filter.Empty;
            return (await _dbSet.FindAsync(filter)).ToEnumerable();
        }

        protected async Task TrySaveChangesAsync(bool isTransactional)
        {
            if (!isTransactional)
            {
                await _mongoDbContext.SaveChangesAsync();
            }
        }
    }
}

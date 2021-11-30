using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;
using Onion.Application.DataAccess.Entities;
using Onion.Application.DataAccess.Repositories;
using Onion.Infrastucture.DataAccess.MongoDb.Configuration;
using Onion.Infrastucture.DataAccess.MongoDb;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Onion.Core.Clock;

namespace Onion.Infrastucture.DataAccess.MongoDb.Repositories
{
    public abstract class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity
    {
        protected readonly IMongoCollection<T> _dbSet;
        protected readonly IMongoDbContext _mongoDbContext;

        public GenericRepository(IMongoDbContext mongoDbContext)
        {
            _dbSet = mongoDbContext.Collection<T>();
            _mongoDbContext = mongoDbContext;
        }

        public async Task<T> CreateAsync(T newEntity)
        {
            _mongoDbContext.SetAuditDates(newEntity, true);

            await _dbSet.InsertOneAsync(newEntity);
            return newEntity;
        }

        public async Task<T> UpdateAsync(T updatedEntity)
        {
            _mongoDbContext.SetAuditDates(updatedEntity);

            var filter = Builders<T>.Filter.Eq(e => e.Id, updatedEntity.Id);
            await _dbSet.ReplaceOneAsync(filter, updatedEntity);

            return updatedEntity;
        }

        public async Task<T> DeleteAsync(T entityToDelete)
        {
            var filter = Builders<T>.Filter.Eq(e => e.Id, entityToDelete.Id);
            await _dbSet.DeleteOneAsync(filter);

            return entityToDelete;
        }

        public async Task<T> GetByIdAsync(Guid entityId)
        {
            var filter = Builders<T>.Filter.Eq(e => e.Id, entityId);
            return await (await _dbSet.FindAsync(filter)).FirstOrDefaultAsync();
        }

        public async Task<IList<T>> ListAsync()
        {
            var filter = Builders<T>.Filter.Empty;
            return await (await _dbSet.FindAsync(filter)).ToListAsync();
        }
    }
}

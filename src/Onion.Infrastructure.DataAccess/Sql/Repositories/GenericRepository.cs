using Microsoft.EntityFrameworkCore;
using Onion.Application.DataAccess.Entities;
using Onion.Application.DataAccess.Exceptions.Common;
using Onion.Application.DataAccess.Repositories;
using Onion.Core.Helpers;
using Onion.Core.Structures;

namespace Onion.Infrastructure.DataAccess.Sql.Repositories;

public abstract class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity
{
    protected readonly SqlDbContext _dbContext;
    protected readonly DbSet<T> _dbSet;

    public GenericRepository(SqlDbContext dbContext)
    {
        _dbContext = dbContext;
        _dbSet = _dbContext.Set<T>();
    }
    public async Task<T> GetByIdAsync(Guid entityId)
    {
        return await _dbSet.FirstOrDefaultAsync(e => e.Id == entityId);
    }

    public async Task<IEnumerable<T>> ListAsync()
    {
        return await _dbSet.ToListAsync();
    }

    public async Task<int> CountAsync()
    {
        return await _dbSet.CountAsync();
    }

    public async Task<PaginableList<T>> PaginateAsync(int pageSize, int page)
    {
        Guard.Min(pageSize, 1, nameof(pageSize));
        Guard.Min(page, 1, nameof(page));

        return await PaginateAsync(pageSize, page, e => true);
    }

    public async Task<T> CreateAsync(T newEntity)
    {
        Guard.NotNull(newEntity, nameof(newEntity));

        T createdEntity = _dbSet.Add(newEntity).Entity;
        await _dbContext.SaveChangesAsync();
        return createdEntity;
    }

    public async Task<T> UpdateAsync(T updatedEntity)
    {
        Guard.NotNull(updatedEntity, nameof(updatedEntity));

        await _dbContext.SaveChangesAsync();
        return updatedEntity;
    }

    public async Task<T> DeleteAsync(T entityToDelete)
    {
        Guard.NotNull(entityToDelete, nameof(entityToDelete));

        T deletedEntity = _dbSet.Remove(entityToDelete).Entity;
        await _dbContext.SaveChangesAsync();
        return deletedEntity;
    }

    protected async Task<PaginableList<T>> PaginateAsync(int pageSize, int page, Func<T, bool> filter)
    {
        Guard.Min(pageSize, 1, nameof(pageSize));
        Guard.Min(page, 1, nameof(page));
        Guard.NotNull(filter, nameof(filter));

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

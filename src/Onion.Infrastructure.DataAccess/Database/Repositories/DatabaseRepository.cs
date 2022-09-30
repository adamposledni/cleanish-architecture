using Microsoft.EntityFrameworkCore;
using Onion.Core.Helpers;
using Onion.Core.Pagination;
using Onion.Application.DataAccess.Database.Repositories;
using Onion.Application.DataAccess.Database.Entities;
using Onion.Core.Cache;
using System.Diagnostics;
using Onion.Infrastructure.DataAccess.Database.Specifications;

namespace Onion.Infrastructure.DataAccess.Database.Repositories;


// TODO: then include -> doesnt work
// TODO: cache key
// TODO: pagination
public class DatabaseRepository<TEntity> : IDatabaseRepository<TEntity> where TEntity : BaseEntity
{
    private readonly SqlDbContext _dbContext;
    private readonly ICacheService _cacheService;
    private readonly DbSet<TEntity> _dbSet;
    private readonly CacheStrategy _cacheStrategy;


    public DatabaseRepository(SqlDbContext dbContext, ICacheService cacheService, CacheStrategy cacheStrategy)
    {
        _dbContext = dbContext;
        _cacheService = cacheService;
        _cacheStrategy = cacheStrategy;
        _dbSet = _dbContext.Set<TEntity>();        
    }

    public async Task<TEntity> CreateAsync(TEntity newEntity)
    {
        Guard.NotNull(newEntity, nameof(newEntity));

        TEntity createdEntity = _dbSet.Add(newEntity).Entity;
        await _dbContext.SaveChangesAsync();
        return createdEntity;
    }

    public async Task<TEntity> UpdateAsync(TEntity updatedEntity)
    {
        Guard.NotNull(updatedEntity, nameof(updatedEntity));

        await _dbContext.SaveChangesAsync();
        return updatedEntity;
    }

    public async Task<TEntity> DeleteAsync(TEntity entityToDelete)
    {
        Guard.NotNull(entityToDelete, nameof(entityToDelete));

        TEntity deletedEntity = _dbSet.Remove(entityToDelete).Entity;
        await _dbContext.SaveChangesAsync();
        return deletedEntity;
    }

    protected SpecificationBuilder<TEntity> Specification()
    {
        return new SpecificationBuilder<TEntity>();
    }

    protected async Task<TResult> ReadDataAsync<TResult>(ISpecification<TEntity> specification, Func<IQueryable<TEntity>, Task<TResult>> queryableOperation)
    {
        string methodName = queryableOperation.Method.Name;
        return await _cacheService.UseCacheAsync<TResult>(
            _cacheStrategy,
            BuildCacheKey(methodName, specification),
            () => queryableOperation(EvaluateSpecification(_dbSet, specification))
        );
    }

    private IQueryable<TEntity> EvaluateSpecification(IQueryable<TEntity> query, ISpecification<TEntity> specification)
    {
        if (specification == null) return query;

        if (specification.Filter != null)
        {
            query = query.Where(specification.Filter);
        }
        if (specification.OrderBy != null)
        {
            query = query.OrderBy(specification.OrderBy);
        }
        if (specification.Take != null)
        {
            query = query.Take(specification.Take.Value);
        }
        if (specification.Skip != null)
        {
            query = query.Skip(specification.Take.Value);
        }
        query = specification.Includes.Aggregate(query, (current, include) => current.Include(include));


        return query;
    }

    private CacheKey BuildCacheKey(string methodName, ISpecification<TEntity> specification)
    {
        return new CacheKey(typeof(TEntity).Name, methodName, null);
    }

    //protected async Task<TEntity> GetByIdAsync(Guid entityId)
    //{
    //    return await ReadDataAsync(
    //        new GetByIdSpecification<TEntity>(entityId),
    //        q => q.SingleOrDefaultAsync()
    //    );
    //}

    //protected async Task<TEntity> GetAsync(ISpecification<TEntity> specification)
    //{
    //    return await ReadDataAsync(
    //        specification,
    //        q => q.SingleOrDefaultAsync()
    //    );
    //}

    //protected async Task<bool> AllAsync(ISpecification<TEntity> specification)
    //{
    //    return await ReadDataAsync(
    //        specification,
    //        q => q.AllAsync(_ => true)
    //    );
    //}

    //public async Task<bool> AnyAsync(ISpecification<TEntity> specification = null)
    //{
    //    return await ReadDataAsync(
    //        specification,
    //        q => q.AnyAsync(_ => true)
    //    );
    //}

    //public async Task<int> CountAsync(ISpecification<TEntity> specification = null)
    //{
    //    return await ReadDataAsync(
    //        specification,
    //        q => q.CountAsync()
    //    );
    //}

    //public async Task<IEnumerable<TEntity>> ListAsync(ISpecification<TEntity> specification = null)
    //{
    //    return await ReadDataAsync(
    //        specification,
    //        q => q.ToListAsync()
    //    );
    //}

    




    //public async Task<PaginableList<T>> PaginateAsync(int pageSize, int page)
    //{
    //    Guard.Min(pageSize, 1, nameof(pageSize));
    //    Guard.Min(page, 1, nameof(page));

    //    return await PaginateAsync(pageSize, page, e => true);
    //}


    //protected async Task<PaginableList<T>> PaginateAsync(int pageSize, int page, Func<T, bool> filter)
    //{
    //    Guard.Min(pageSize, 1, nameof(pageSize));
    //    Guard.Min(page, 1, nameof(page));
    //    Guard.NotNull(filter, nameof(filter));

    //    int countOfEntities = await CountAsync();
    //    int countOfPages = (int)Math.Ceiling((double)countOfEntities / pageSize);
    //    if (page > countOfPages) throw new PageOutOfBoundsException();

    //    var entities = await _dbSet
    //        .Where(e => filter(e))
    //        .Skip(pageSize * (page - 1)).Take(pageSize)
    //        .ToListAsync();

    //    return new PaginableList<T>(entities, countOfEntities, pageSize, page, countOfPages);
    //}
}
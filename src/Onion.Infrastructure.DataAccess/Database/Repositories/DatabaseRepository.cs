using Microsoft.EntityFrameworkCore;
using Onion.Core.Helpers;
using Onion.Core.Pagination;
using Onion.Application.DataAccess.Database.Repositories;
using Onion.Application.DataAccess.Database.Entities;
using Onion.Core.Cache;
using Onion.Infrastructure.DataAccess.Database.Specifications;
using System.Runtime.CompilerServices;
using Onion.Core.Extensions;

namespace Onion.Infrastructure.DataAccess.Database.Repositories;

// TODO: test cache key
// TODO: pagination
public class DatabaseRepository<TEntity> : IDatabaseRepository<TEntity> where TEntity : BaseEntity
{
    private readonly SqlDbContext _dbContext;
    private readonly ICacheService _cacheService;
    private readonly DbSet<TEntity> _dbSet;

    private readonly SetOnce<CacheStrategy> _setOnceCacheStrategy = new();
    public CacheStrategy CacheStrategy 
    {
        get { return _setOnceCacheStrategy.Value; }
        set { _setOnceCacheStrategy.Value = value; }
    }

    public DatabaseRepository(SqlDbContext dbContext, ICacheService cacheService)
    {
        _dbContext = dbContext;
        _cacheService = cacheService;
        _dbSet = _dbContext.Set<TEntity>();        
    }

    public async Task<TEntity> CreateAsync(TEntity newEntity, bool commitAfter = true)
    {
        Guard.NotNull(newEntity, nameof(newEntity));

        TEntity createdEntity = _dbSet.Add(newEntity).Entity;
        await CommitIfTrueAsync(commitAfter);
        return createdEntity;
    }

    public async Task<TEntity> UpdateAsync(TEntity updatedEntity, bool commitAfter = true)
    {
        Guard.NotNull(updatedEntity, nameof(updatedEntity));

        await CommitIfTrueAsync(commitAfter);
        return updatedEntity;
    }

    public async Task<TEntity> DeleteAsync(TEntity entityToDelete, bool commitAfter = true)
    {
        Guard.NotNull(entityToDelete, nameof(entityToDelete));

        TEntity deletedEntity = _dbSet.Remove(entityToDelete).Entity;
        await _dbContext.SaveChangesAsync();
        return deletedEntity;
    }

    private async Task<int> CommitIfTrueAsync(bool shouldCommit)
    {
        if (shouldCommit) return await _dbContext.SaveChangesAsync();
        return 0;
    }

    protected SpecificationBuilder<TEntity> Specification() => new SpecificationBuilder<TEntity>();

    protected async Task<TResult> ReadDataAsync<TResult>(Func<IQueryable<TEntity>, Task<TResult>> queryOperation, [CallerMemberName] string callerMethodName = "")
    {
        return await ReadDataAsync(Specification().Build(), queryOperation, callerMethodName);
    }

    protected async Task<TResult> ReadDataAsync<TResult>(ISpecification<TEntity> specification, Func<IQueryable<TEntity>, Task<TResult>> queryOperation, [CallerMemberName] string callerMethodName = "")
    {
        var valueProvider = () => queryOperation(SpecificationEvaluator.Evaluate(_dbSet, specification));
        var cacheKey = new CacheKey(
            typeof(TEntity).Name,
            callerMethodName,
            specification.Filter?.ToEvaluatedString(),
            specification.OrderBy?.ToEvaluatedString(),
            specification.Skip?.ToString(),
            specification.Take?.ToString()
        );

        return await _cacheService.UseCacheAsync(
            CacheStrategy,
            cacheKey,
            valueProvider
        );
    }


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
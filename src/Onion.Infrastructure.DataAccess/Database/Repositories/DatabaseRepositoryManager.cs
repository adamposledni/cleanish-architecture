using Microsoft.Extensions.DependencyInjection;
using Onion.Application.DataAccess.Database.Entities;
using Onion.Application.DataAccess.Database.Repositories;
using Onion.Core.Cache;
using Onion.Core.Helpers;

namespace Onion.Infrastructure.DataAccess.Database.Repositories;

// TODO: Disposable
public class DatabaseRepositoryManager : IDatabaseRepositoryManager
{
    private readonly SqlDbContext _dbContext;
    private readonly IServiceProvider _serviceProvider;

    public DatabaseRepositoryManager(SqlDbContext dbContext, IServiceProvider serviceProvider)
    {
        _dbContext = dbContext;
        _serviceProvider = serviceProvider;
    }

    public TRepository GetRepository<TRepository, TEntity>(CacheStrategy cacheStrategy) 
        where TRepository: IDatabaseRepository<TEntity> where TEntity: BaseEntity
    {
        var repository = _serviceProvider.GetService<TRepository>();
        Guard.NotNull(repository, nameof(repository));

        repository.CacheStrategy = cacheStrategy;
        return repository;
    }

    public async Task<int> CommitAsync()
    {
        return await _dbContext.SaveChangesAsync();
    }
}


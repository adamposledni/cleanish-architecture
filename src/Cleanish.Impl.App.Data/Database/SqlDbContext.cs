using Microsoft.EntityFrameworkCore;
using Cleanish.App.Data.Database.Entities;
using Cleanish.App.Data.Database.Exceptions;
using Cleanish.Shared.Clock;
using System.Reflection;
using System.Threading;

namespace Cleanish.Impl.App.Data.Database;

internal class SqlDbContext : DbContext
{
    private readonly IClockProvider _clockProvider;

    public DbSet<User> Users { get; set; }
    public DbSet<RefreshToken> RefreshTokens { get; set; }
    public DbSet<TodoItem> TodoItems { get; set; }

    public SqlDbContext(DbContextOptions<SqlDbContext> options, IClockProvider clockProvider) : base(options)
    {
        _clockProvider = clockProvider;
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetAssembly(typeof(SqlDbContext)));
        base.OnModelCreating(modelBuilder);
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var entries = ChangeTracker.Entries().Where(e => e.State == EntityState.Added || e.State == EntityState.Modified);

        foreach (var entityEntry in entries)
        {
            BaseEntity entity = (BaseEntity)entityEntry.Entity;
            if (entityEntry.State == EntityState.Added)
            {
                entity.Created = _clockProvider.Now;
                entity.Updated = _clockProvider.Now;
            }
            else if (entityEntry.State == EntityState.Modified)
            {
                entity.Updated = _clockProvider.Now;
            }
        }

        try
        {
            return await base.SaveChangesAsync(cancellationToken);
        }
        catch (DbUpdateConcurrencyException)
        {
            throw new DataUpdateConcurrencyException();
        }    
    }
}

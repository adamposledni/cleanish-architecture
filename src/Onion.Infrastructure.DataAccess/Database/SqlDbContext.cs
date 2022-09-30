using Microsoft.EntityFrameworkCore;
using Onion.Application.DataAccess.Database.Entities;
using Onion.Application.Services.Auth.Models;
using Onion.Core.Clock;
using System.Reflection;
using System.Threading;

namespace Onion.Infrastructure.DataAccess.Database;

public class SqlDbContext : DbContext
{
    private readonly IClockProvider _clockProvider;

    public DbSet<User> Users { get; set; }
    public DbSet<RefreshToken> RefreshTokens { get; set; }
    public DbSet<TodoList> TodoLists { get; set; }
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

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
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

        return base.SaveChangesAsync(cancellationToken);
    }
}

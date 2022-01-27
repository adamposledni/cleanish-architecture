using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Onion.Application.DataAccess.Entities;

namespace Onion.Infrastructure.DataAccess.Sql.EntityConfigurations;

public abstract class EntityTypeConfiguration<T> : IEntityTypeConfiguration<T> where T : BaseEntity
{
    public virtual void Configure(EntityTypeBuilder<T> builder)
    {
        builder.ToTable(typeof(T).Name);
        builder.HasKey(e => e.Id);
    }
}

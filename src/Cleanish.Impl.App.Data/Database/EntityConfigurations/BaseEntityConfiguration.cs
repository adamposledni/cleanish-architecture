using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Cleanish.App.Data.Database.Entities;
using Cleanish.Shared.Helpers;

namespace Cleanish.Impl.App.Data.Database.EntityConfigurations;

internal abstract class BaseEntityConfiguration<T> 
    : IEntityTypeConfiguration<T> where T : BaseEntity
{
    public virtual void Configure(EntityTypeBuilder<T> builder)
    {
        Guard.NotNull(builder, nameof(builder));

        builder.ToTable(typeof(T).Name);
        builder.HasKey(e => e.Id);
        builder.Property(e => e.Version).IsRowVersion();
    }
}

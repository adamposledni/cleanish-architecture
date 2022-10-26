using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Onion.App.Data.Database.Entities;
using Onion.Shared.Helpers;

namespace Onion.Impl.App.Data.Database.EntityConfigurations;

internal abstract class BaseEntityConfiguration<T> : IEntityTypeConfiguration<T> where T : BaseEntity
{
    public virtual void Configure(EntityTypeBuilder<T> builder)
    {
        Guard.NotNull(builder, nameof(builder));

        builder.ToTable(typeof(T).Name);
        builder.HasKey(e => e.Id);
        builder.Property(e => e.Version).IsRowVersion();
    }
}

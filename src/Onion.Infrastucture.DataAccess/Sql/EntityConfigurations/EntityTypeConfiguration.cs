using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Onion.Application.DataAccess.Database.Entities;

namespace Onion.Infrastucture.DataAccess.Sql.EntityConfigurations
{
    public abstract class EntityTypeConfiguration<T> : IEntityTypeConfiguration<T> where T : DatabaseEntity
    {
        public virtual void Configure(EntityTypeBuilder<T> builder)
        {
            builder.ToTable(typeof(T).Name);
            builder.HasKey(e => e.Id);
        }
    }
}

using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Onion.Application.DataAccess.Entities;

namespace Onion.Infrastucture.DataAccess.Sql.EntityConfigurations
{
    public class ItemConfiguration : EntityTypeConfiguration<Item>
    {
        public override void Configure(EntityTypeBuilder<Item> builder)
        {
            base.Configure(builder);

            builder.Property(i => i.Title).IsRequired();
        }
    }
}

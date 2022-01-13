using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Onion.Application.DataAccess.Entities;

namespace Onion.Infrastucture.DataAccess.Sql.EntityConfigurations
{
    public class UserConfiguration : EntityTypeConfiguration<User>
    {
        public override void Configure(EntityTypeBuilder<User> builder)
        {
            base.Configure(builder);

            builder.Property(i => i.Email).IsRequired();
        }
    }
}

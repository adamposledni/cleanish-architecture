using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Onion.Application.DataAccess.Entities;
using Onion.Core.Helpers;

namespace Onion.Infrastructure.DataAccess.Sql.EntityConfigurations;

public class UserConfiguration : BaseEntityConfiguration<User>
{
    public override void Configure(EntityTypeBuilder<User> builder)
    {
        Guard.NotNull(builder, nameof(builder));

        base.Configure(builder);

        builder.Property(i => i.Email).IsRequired();
    }
}

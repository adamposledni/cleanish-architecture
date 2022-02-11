using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Onion.Application.DataAccess.Entities;
using Onion.Core.Helpers;

namespace Onion.Infrastructure.DataAccess.Sql.EntityConfigurations;

public class RefreshTokenConfiguration : BaseEntityConfiguration<RefreshToken>
{
    public override void Configure(EntityTypeBuilder<RefreshToken> builder)
    {
        Guard.NotNull(builder, nameof(builder));

        base.Configure(builder);

        builder.Property(i => i.Token).IsRequired();
        builder.HasIndex(i => i.Token).IsUnique();
    }
}
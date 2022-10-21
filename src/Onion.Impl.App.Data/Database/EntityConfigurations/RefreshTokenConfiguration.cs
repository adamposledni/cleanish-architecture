using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Onion.App.Data.Database.Entities;
using Onion.Shared.Helpers;

namespace Onion.Impl.App.Data.Database.EntityConfigurations;

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
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Cleanish.App.Data.Database.Entities;
using Cleanish.Shared.Helpers;

namespace Cleanish.Impl.App.Data.Database.EntityConfigurations;

internal class RefreshTokenConfiguration : BaseEntityConfiguration<RefreshToken>
{
    public override void Configure(EntityTypeBuilder<RefreshToken> builder)
    {
        Guard.NotNull(builder, nameof(builder));

        base.Configure(builder);

        builder.Property(i => i.Token).IsRequired();
        builder.HasIndex(i => i.Token).IsUnique();
    }
}
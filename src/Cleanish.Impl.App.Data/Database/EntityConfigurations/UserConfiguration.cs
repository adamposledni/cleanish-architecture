using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Cleanish.App.Data.Database.Entities;
using Cleanish.Shared.Helpers;

namespace Cleanish.Impl.App.Data.Database.EntityConfigurations;

internal class UserConfiguration : BaseEntityConfiguration<User>
{
    public override void Configure(EntityTypeBuilder<User> builder)
    {
        Guard.NotNull(builder, nameof(builder));

        base.Configure(builder);

        builder.Property(i => i.Email).IsRequired();
        builder.HasIndex(i => i.Email).IsUnique();
    }
}

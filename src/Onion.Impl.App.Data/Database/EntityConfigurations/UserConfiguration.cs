using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Onion.App.Data.Database.Entities;
using Onion.Shared.Helpers;

namespace Onion.Impl.App.Data.Database.EntityConfigurations;

internal class UserConfiguration : BaseEntityConfiguration<User>
{
    public override void Configure(EntityTypeBuilder<User> builder)
    {
        Guard.NotNull(builder, nameof(builder));

        base.Configure(builder);

        builder.Property(i => i.Email).IsRequired();
        builder.HasIndex(i => i.Email).IsUnique();
        builder.HasIndex(i => i.GoogleSubjectId).IsUnique();
    }
}

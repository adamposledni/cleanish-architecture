using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Onion.Application.DataAccess.Database.Entities;
using Onion.Core.Helpers;

namespace Onion.Infrastructure.DataAccess.Database.EntityConfigurations;

public class UserConfiguration : BaseEntityConfiguration<User>
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

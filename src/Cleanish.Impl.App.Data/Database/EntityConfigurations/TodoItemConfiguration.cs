using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Cleanish.App.Data.Database.Entities;
using Cleanish.Shared.Helpers;

namespace Cleanish.Impl.App.Data.Database.EntityConfigurations;

internal class TodoItemConfiguration : BaseEntityConfiguration<TodoItem>
{
    public override void Configure(EntityTypeBuilder<TodoItem> builder)
    {
        Guard.NotNull(builder, nameof(builder));

        base.Configure(builder);

        builder.Property(i => i.Title)
            .IsRequired()
            .HasMaxLength(50);
    }
}
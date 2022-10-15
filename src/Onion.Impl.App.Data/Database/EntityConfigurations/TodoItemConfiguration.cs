using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Onion.App.Data.Database.Entities;
using Onion.Shared.Helpers;

namespace Onion.Impl.App.Data.Database.EntityConfigurations;

public class TodoItemConfiguration : BaseEntityConfiguration<TodoItem>
{
    public override void Configure(EntityTypeBuilder<TodoItem> builder)
    {
        Guard.NotNull(builder, nameof(builder));

        base.Configure(builder);

        builder.Property(i => i.Title).IsRequired();
    }
}
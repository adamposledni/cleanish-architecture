using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Onion.Application.DataAccess.Database.Entities;
using Onion.Core.Helpers;

namespace Onion.Infrastructure.DataAccess.Database.EntityConfigurations;

public class TodoListConfiguration : BaseEntityConfiguration<TodoList>
{
    public override void Configure(EntityTypeBuilder<TodoList> builder)
    {
        Guard.NotNull(builder, nameof(builder));

        base.Configure(builder);

        builder.Property(i => i.Title).IsRequired();
        builder.HasIndex(i => i.Title).IsUnique();
    }
}
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Onion.Application.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Onion.Infrastucture.Persistence.EntityConfigurations
{
    public class ItemConfiguration: EntityTypeConfiguration<Item>
    {
        public override void Configure(EntityTypeBuilder<Item> builder)
        {
            base.Configure(builder);

            builder.Property(i => i.Title).IsRequired();
        }
    }
}

using MongoDB.Bson.Serialization;
using Onion.Application.DataAccess.Entities;

namespace Onion.Infrastructure.DataAccess.MongoDb.EntityConfigurations;

public class UserConfiguration : EntityTypeConfiguration
{
    public override void Configure()
    {
        BsonClassMap.RegisterClassMap<User>((map) =>
        {
            Configure(map);
        });
    }
}

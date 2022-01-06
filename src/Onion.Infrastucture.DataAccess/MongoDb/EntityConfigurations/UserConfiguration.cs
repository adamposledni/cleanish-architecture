using MongoDB.Bson.Serialization;
using Onion.Application.DataAccess.Entities;

namespace Onion.Infrastucture.DataAccess.MongoDb.EntityConfigurations
{
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
}

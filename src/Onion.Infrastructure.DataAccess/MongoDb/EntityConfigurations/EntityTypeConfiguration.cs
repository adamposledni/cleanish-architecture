using MongoDB.Bson.Serialization;
using Onion.Application.DataAccess.Entities;

namespace Onion.Infrastructure.DataAccess.MongoDb.EntityConfigurations;

public abstract class EntityTypeConfiguration
{
    protected void Configure<T>(BsonClassMap<T> map) where T : BaseEntity
    {
        map.AutoMap();
        map.SetIdMember(map.GetMemberMap(e => e.Id));
    }

    public abstract void Configure();
}

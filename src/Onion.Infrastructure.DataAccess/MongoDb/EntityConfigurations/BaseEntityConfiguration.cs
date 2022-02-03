using MongoDB.Bson.Serialization;
using Onion.Application.DataAccess.Entities;
using Onion.Core.Helpers;

namespace Onion.Infrastructure.DataAccess.MongoDb.EntityConfigurations;

public abstract class BaseEntityConfiguration
{
    protected void Configure<T>(BsonClassMap<T> map) where T : BaseEntity
    {
        Guard.NotNull(map, nameof(map));

        map.AutoMap();
        map.SetIdMember(map.GetMemberMap(e => e.Id));
    }

    public abstract void Configure();
}

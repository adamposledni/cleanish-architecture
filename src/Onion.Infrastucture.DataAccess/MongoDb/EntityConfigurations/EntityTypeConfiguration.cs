using MongoDB.Bson.Serialization;
using Onion.Application.DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Onion.Infrastucture.DataAccess.MongoDb.EntityConfigurations
{
    public class EntityTypeConfiguration
    {
        protected static Action<BsonClassMap<T>> Configure<T>() where T : BaseEntity
        {
            Action<BsonClassMap<T>> act = (map) =>
            {
                map.AutoMap();
                map.SetIdMember(map.GetMemberMap(e => e.Id));
            };
            return act;
        }
    }
}

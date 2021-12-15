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
        protected static void Configure<T>(BsonClassMap<T> map) where T : BaseEntity
        {
            map.AutoMap();
            map.SetIdMember(map.GetMemberMap(e => e.Id));
        }
    }
}

using MongoDB.Bson.Serialization;
using Onion.Application.DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Onion.Infrastucture.DataAccess.MongoDb.EntityConfigurations
{
    public class UserConfiguration : EntityTypeConfiguration
    {
        public static void Configure()
        {
            BsonClassMap.RegisterClassMap<User>((map) =>
            {
                Configure(map);
            });
        }
    }
}

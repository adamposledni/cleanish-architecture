using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Onion.Application.DataAccess.Entities;
using Onion.Core.Clock;
using Onion.Infrastucture.DataAccess.MongoDb.EntityConfigurations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Onion.Infrastucture.DataAccess.MongoDb
{
    public class MongoDbContext : IMongoDbContext
    {
        private readonly MongoClient _mongoClient;
        private readonly IMongoDatabase _mongoDatabase;
        private readonly IClockProvider _clockProvider;

        public MongoDbContext(IOptions<MongoDbSettings> mongoDbSettings, IClockProvider clockProvider)
        {
            _mongoClient = new MongoClient(mongoDbSettings.Value.ConnectionString);
            _mongoDatabase = _mongoClient.GetDatabase(mongoDbSettings.Value.DatabaseName);
            _clockProvider = clockProvider;
        }

        public IMongoCollection<T> Collection<T>()
        {
            return _mongoDatabase.GetCollection<T>(typeof(T).Name);
        }

        public void SetAuditDates(BaseEntity entity, bool created = false)
        {
            if (created) entity.Created = _clockProvider.Now;
            entity.Updated = _clockProvider.Now;
        }

        public static void Configure()
        {
            UserConfiguration.Configure();
        }
    }
}

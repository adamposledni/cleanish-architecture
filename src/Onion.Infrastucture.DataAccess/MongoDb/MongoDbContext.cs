using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Onion.Application.DataAccess.Entities;
using Onion.Infrastucture.DataAccess.MongoDb.Configuration;
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

        public MongoDbContext(MongoDbSettings mongoDbSettings)
        {
            _mongoClient = new MongoClient(mongoDbSettings.ConnectionString);
            _mongoDatabase = _mongoClient.GetDatabase(mongoDbSettings.DatabaseName);

            ApplyEntityConfigurations();
        }

        public IMongoCollection<T> Collection<T>()
        {
            return _mongoDatabase.GetCollection<T>(typeof(T).Name);
        }

        private void ApplyEntityConfigurations()
        {
            UserConfiguration.Configure();
        }
    }
}

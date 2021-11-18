using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Onion.Infrastucture.DataAccess.NoSql.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Onion.Infrastucture.DataAccess.NoSql
{   
    // TODO: dispose, commit, audit date
    public class MongoDbContext
    {
        private readonly MongoClient _mongoClient;
        private readonly IMongoDatabase _mongoDatabase;
        private readonly IList<Func<Task>> _commands;

        public MongoDbContext(IOptions<MongoDbSettings> mongoDbSettings)
        {
            _mongoClient = new MongoClient(mongoDbSettings.Value.ConnectionString);
            _mongoDatabase = _mongoClient.GetDatabase(mongoDbSettings.Value.DatabaseName);
            _commands = new List<Func<Task>>();
        }

        public async Task SaveChangesAsync()
        {
            using var session = await _mongoClient.StartSessionAsync();

            session.StartTransaction();

            // execute commands

            await session.CommitTransactionAsync();
        }

        public IMongoCollection<T> Collection<T>()
        {
            return _mongoDatabase.GetCollection<T>(typeof(T).Name);
        }

        public void AddCommand(Func<Task> command)
        {
            _commands.Add(command);
        }
    }
}

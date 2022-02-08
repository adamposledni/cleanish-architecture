using MongoDB.Driver;
using Onion.Application.DataAccess.Entities;
using Onion.Core.Clock;
using Onion.Core.Extensions;
using Onion.Core.Helpers;
using Onion.Infrastructure.DataAccess.MongoDb.EntityConfigurations;

namespace Onion.Infrastructure.DataAccess.MongoDb;

public class MongoDbContext : IMongoDbContext
{
    private readonly MongoClient _mongoClient;
    private readonly IMongoDatabase _mongoDatabase;
    private readonly IClockProvider _clockProvider;

    public MongoDbContext(MongoDbContextOptions options, IClockProvider clockProvider)
    {
        MongoUrl connectionString = new(options.ConnectionString);
        _mongoClient = new MongoClient(connectionString);
        _mongoDatabase = _mongoClient.GetDatabase(connectionString.DatabaseName);
        _clockProvider = clockProvider;
    }

    public IMongoCollection<T> Collection<T>()
    {
        return _mongoDatabase.GetCollection<T>(typeof(T).Name);
    }

    public void SetAuditDates(BaseEntity entity, bool created = false)
    {
        Guard.NotNull(entity, nameof(entity));

        if (created) entity.Created = _clockProvider.Now;
        entity.Updated = _clockProvider.Now;
    }

    public static void Configure()
    {
        var assembly = typeof(MongoDbContext).Assembly;
        var configurationTypes = assembly.GetDerivedTypes<BaseEntityConfiguration>();

        foreach (var configurationType in configurationTypes)
        {
            var instance = (BaseEntityConfiguration)Activator.CreateInstance(configurationType);
            instance.Configure();
        }
    }
}

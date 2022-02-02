using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Onion.Application.DataAccess.Entities;
using Onion.Core.Clock;
using Onion.Core.Extensions;
using Onion.Infrastructure.DataAccess.MongoDb.EntityConfigurations;

namespace Onion.Infrastructure.DataAccess.MongoDb;

public class MongoDbContext : IMongoDbContext
{
    private readonly MongoDbSettings _mongoDbSettings;
    private readonly MongoClient _mongoClient;
    private readonly IMongoDatabase _mongoDatabase;
    private readonly IClockProvider _clockProvider;

    public MongoDbContext(IOptions<MongoDbSettings> mongoDbSettings, IClockProvider clockProvider)
    {
        _mongoDbSettings = mongoDbSettings.Value;
        _mongoClient = new MongoClient(_mongoDbSettings.ConnectionString);
        _mongoDatabase = _mongoClient.GetDatabase(_mongoDbSettings.DatabaseName);
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
        var assembly = typeof(MongoDbContext).Assembly;
        var configurationTypes = assembly.GetDerivedTypes<EntityTypeConfiguration>();

        foreach (var configurationType in configurationTypes)
        {
            var instance = (EntityTypeConfiguration)Activator.CreateInstance(configurationType);
            instance.Configure();
        }
    }
}

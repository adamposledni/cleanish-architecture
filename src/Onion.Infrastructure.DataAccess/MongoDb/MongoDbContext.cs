using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using Onion.Application.DataAccess.Entities;
using Onion.Core.Clock;
using Onion.Core.Extensions;
using Onion.Infrastructure.DataAccess.MongoDb.EntityConfigurations;

namespace Onion.Infrastructure.DataAccess.MongoDb;

public class MongoDbContext : IMongoDbContext
{
    private readonly MongoDbSettings _settings;
    private readonly MongoClient _mongoClient;
    private readonly IMongoDatabase _mongoDatabase;
    private readonly IClockProvider _clockProvider;

    public MongoDbContext(IConfiguration configuration, IClockProvider clockProvider)
    {
        _settings = configuration.GetMongoDbSettings();
        _mongoClient = new MongoClient(_settings.ConnectionString);
        _mongoDatabase = _mongoClient.GetDatabase(_settings.DatabaseName);
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

namespace Onion.Infrastructure.DataAccess.MongoDb;

public class MongoDbContextOptions
{
    public string ConnectionString { get; set; }

    public MongoDbContextOptions(string connectionString)
    {
        ConnectionString = connectionString;
    }
}

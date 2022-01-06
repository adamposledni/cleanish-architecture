using MongoDB.Driver;
using Onion.Application.DataAccess.Entities;

namespace Onion.Infrastucture.DataAccess.MongoDb
{
    public interface IMongoDbContext
    {
        IMongoCollection<T> Collection<T>();
        void SetAuditDates(BaseEntity entity, bool created = false);
    }
}
using MongoDB.Driver;
using Onion.Application.DataAccess.Entities;
using System;
using System.Threading.Tasks;

namespace Onion.Infrastucture.DataAccess.MongoDb
{
    public interface IMongoDbContext
    {
        IMongoCollection<T> Collection<T>();
        void SetAuditDates(BaseEntity entity, bool created = false);
    }
}
using Onion.Application.DataAccess.Repositories;
using Onion.Infrastucture.DataAccess.MongoDb;
using Onion.Infrastucture.DataAccess.Sql;
using System.Threading.Tasks;

namespace Onion.Infrastucture.DataAccess
{
    public class TransactionalRepositoryManager : RepositoryManager, ITransactionalRepositoryManager
    {
        public TransactionalRepositoryManager(IMongoDbContext mongoDbContext, SqlDbContext sqlDbContext)
            : base(mongoDbContext, sqlDbContext)
        {
            _isTransactional = true;
        }

        public async Task SaveChangesAsync()
        {
            await _sqlDbContext.SaveChangesAsync();
        }
    }
}

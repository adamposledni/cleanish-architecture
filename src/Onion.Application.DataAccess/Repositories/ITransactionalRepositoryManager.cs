using System.Threading.Tasks;

namespace Onion.Application.DataAccess.Repositories
{
    public interface ITransactionalRepositoryManager : IRepositoryManager
    {
        Task SaveChangesAsync();
    }
}

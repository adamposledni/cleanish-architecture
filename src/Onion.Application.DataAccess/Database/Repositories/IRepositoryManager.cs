using System.Threading.Tasks;

namespace Onion.Application.DataAccess.Database.Repositories
{
    public interface IRepositoryManager
    {
        IItemRepository ItemRepository { get; }
        Task SaveChangesAsync();
    }
}

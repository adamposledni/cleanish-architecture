using System.Threading.Tasks;

namespace Onion.Application.DataAccess.Repositories
{
    public interface IRepositoryManager
    {
        IItemRepository ItemRepository { get; }
        IUserRepository UserRepository { get; }
    }
}

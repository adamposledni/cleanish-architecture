using Onion.Application.DataAccess.Repositories;
using System.Threading.Tasks;

namespace Onion.Application.DataAccess.Repositoies
{
    public interface IRepositoryManager
    {
        IItemRepository ItemRepository { get; }
        IUserRepository UserRepository { get; }
    }
}

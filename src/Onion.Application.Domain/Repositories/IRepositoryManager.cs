using System;
using System.Threading.Tasks;

namespace Onion.Application.Domain.Repositories
{
    public interface IRepositoryManager
    {
        IItemRepository ItemRepository { get; }
        Task SaveChangesAsync();
    }
}

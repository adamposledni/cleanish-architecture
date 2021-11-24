using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Onion.Application.DataAccess.Repositories
{
    public interface ITransactionalRepositoryManager : IRepositoryManager
    {
        Task SaveChangesAsync();
    }
}

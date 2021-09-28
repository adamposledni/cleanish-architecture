using Microsoft.EntityFrameworkCore;
using Onion.Application.Domain.Entities;
using Onion.Application.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Onion.Infrastucture.Persistence.Repositories
{
    public class ItemRepository: GenericRepository<Item>, IItemRepository
    {
        public ItemRepository(OnionDbContext dbContext): base(dbContext)
        {
        }
    }
}

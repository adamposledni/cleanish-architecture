using Onion.Application.Services.Models.Item;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Onion.Application.Services.Abstractions
{
    public interface IItemService
    {
        Task<IEnumerable<ItemRes>> ListAsync();
        Task<ItemRes> GetAsync(int itemId);
        Task<ItemRes> CreateAsync(ItemReq newItem);
        Task<ItemRes> DeleteAsync(int itemId);
        //Task<ItemRes> UpdateAsync(int itemId, ItemReq updatedItem);

        Task<bool> FooAsync();
    }
}

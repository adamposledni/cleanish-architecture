using Onion.Application.DataAccess.Database.Entities;
using Onion.Application.DataAccess.Database.Repositories;
using Onion.Application.Services.Abstractions;
using Onion.Application.Services.Exceptions;
using Onion.Application.Services.Models.Item;
using Onion.Core.Mapper;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Onion.Application.Services.Implementations
{
    public class ItemService : IItemService
    {
        private readonly IRepositoryManager _repositoryManager;
        private readonly IMapper _mapper;

        public ItemService(IRepositoryManager repositoryManager, IMapper mapper)
        {
            _repositoryManager = repositoryManager;
            _mapper = mapper;
        }

        public async Task<ItemRes> CreateAsync(ItemReq newItem)
        {
            Item newItemEntity = _mapper.Map<ItemReq, Item>(newItem);
            newItemEntity = await _repositoryManager.ItemRepository.CreateAsync(newItemEntity);

            return _mapper.Map<Item, ItemRes>(newItemEntity);
        }

        public async Task<ItemRes> DeleteAsync(int itemId)
        {
            Item itemToDelete = await _repositoryManager.ItemRepository.GetByIdAsync(itemId);
            if (itemToDelete == null) throw new ItemNotFoundException(itemId);

            itemToDelete = await _repositoryManager.ItemRepository.DeleteAsync(itemToDelete);

            return _mapper.Map<Item, ItemRes>(itemToDelete);
        }

        public async Task<ItemRes> GetAsync(int itemId)
        {
            var item = await _repositoryManager.ItemRepository.GetByIdAsync(itemId);
            if (item == null) throw new ItemNotFoundException(itemId);
            return _mapper.Map<Item, ItemRes>(item);
        }

        public async Task<IEnumerable<ItemRes>> ListAsync()
        {
            var items = await _repositoryManager.ItemRepository.ListAsync();
            return items.Select(i => _mapper.Map<Item, ItemRes>(i));
        }

        public async Task<ItemRes> UpdateAsync(int itemId, ItemReq updatedItem)
        {
            var itemToUpdate = await _repositoryManager.ItemRepository.GetByIdAsync(itemId);
            if (itemToUpdate == null) throw new ItemNotFoundException(itemId);

            itemToUpdate.Title = updatedItem.Title;
            itemToUpdate.Description = updatedItem.Description;

            await _repositoryManager.ItemRepository.UpdateAsync(itemToUpdate);
            return _mapper.Map<Item, ItemRes>(itemToUpdate);
        }

        public Task<bool> FooAsync()
        {
            return Task.FromResult(true);
        }
    }
}

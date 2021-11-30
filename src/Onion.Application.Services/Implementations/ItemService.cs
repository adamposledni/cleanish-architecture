using Onion.Application.DataAccess.Entities;
using Onion.Application.DataAccess.Repositories;
using Onion.Application.Services.Abstractions;
using Onion.Application.Services.Exceptions;
using Onion.Application.Services.Models.Item;
using Onion.Core.Mapper;
using System;
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

        public async Task<ItemRes> GetAsync(Guid itemId)
        {
            var item = await _repositoryManager.ItemRepository.GetByIdAsync(itemId);
            if (item == null) throw new ItemNotFoundException(itemId);
            return _mapper.Map<Item, ItemRes>(item);
        }

        public async Task<IList<ItemRes>> ListAsync()
        {
            var items = await _repositoryManager.ItemRepository.ListAsync();
            return items.Select(i => _mapper.Map<Item, ItemRes>(i)).ToList();
        }

        public async Task<ItemRes> CreateAsync(ItemReq model)
        {
            Item newItem = _mapper.Map<ItemReq, Item>(model);
            newItem = await _repositoryManager.ItemRepository.CreateAsync(newItem);

            return _mapper.Map<Item, ItemRes>(newItem);
        }

        public async Task<ItemRes> DeleteAsync(Guid itemId)
        {
            Item itemToDelete = await _repositoryManager.ItemRepository.GetByIdAsync(itemId);
            if (itemToDelete == null) throw new ItemNotFoundException(itemId);

            itemToDelete = await _repositoryManager.ItemRepository.DeleteAsync(itemToDelete);

            return _mapper.Map<Item, ItemRes>(itemToDelete);
        }

        public async Task<ItemRes> UpdateAsync(Guid itemId, ItemReq model)
        {
            var itemToUpdate = await _repositoryManager.ItemRepository.GetByIdAsync(itemId);
            if (itemToUpdate == null) throw new ItemNotFoundException(itemId);

            itemToUpdate.Title = model.Title;
            itemToUpdate.Description = model.Description;

            await _repositoryManager.ItemRepository.UpdateAsync(itemToUpdate);
            return _mapper.Map<Item, ItemRes>(itemToUpdate);
        }

        public Task<bool> FooAsync()
        {
            return Task.FromResult(true);
        }
    }
}

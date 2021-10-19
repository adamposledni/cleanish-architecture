using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Onion.Application.Services.Abstractions;
using Onion.Application.Services.Models.Item;
using Onion.WebApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Onion.WebApi.Controllers
{
    [ApiController]
    [ProducesResponseType(400, Type = typeof(ErrorRes))]
    [ProducesResponseType(500, Type = typeof(ErrorRes))]
    [Produces("application/json")]
    [Route("api/items")]
    public class ItemController : BaseController
    {
        private readonly IItemService _itemService;

        public ItemController(IItemService itemService)
        {
            _itemService = itemService;
        }

        [ProducesResponseType(200)]
        [ProducesResponseType(404, Type = typeof(ErrorRes))]
        [HttpGet("{itemId}")]
        public async Task<ActionResult<ItemRes>> Get([FromRoute] int itemId)
        {
            return StatusCode(200, await _itemService.GetAsync(itemId));
        }

        [ProducesResponseType(200)]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ItemRes>>> List()
        {
            return StatusCode(200, await _itemService.ListAsync());
        }

        [ProducesResponseType(201)]
        [HttpPost]
        public async Task<ActionResult<ItemRes>> Create([FromBody] ItemReq body)
        {
            return StatusCode(201, await _itemService.CreateAsync(body));
        }

        [ProducesResponseType(200)]
        [ProducesResponseType(404, Type = typeof(ErrorRes))]
        [HttpDelete("{itemId}")]
        public async Task<ActionResult<ItemRes>> Delete([FromRoute] int itemId)
        {
            return StatusCode(200, await _itemService.DeleteAsync(itemId));
        }

        [ProducesResponseType(200)]
        [ProducesResponseType(404, Type = typeof(ErrorRes))]
        [HttpPut("{itemId}")]
        public async Task<ActionResult<ItemRes>> Update([FromRoute] int itemId, [FromBody] ItemReq body)
        {
            return StatusCode(200, await _itemService.UpdateAsync(itemId, body));
        }
    }
}

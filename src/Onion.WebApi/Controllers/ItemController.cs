using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Onion.Application.Services.ItemManagement;
using Onion.Application.Services.ItemManagement.Models;
using Onion.Core.Structures;
using Onion.WebApi.Models;
using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace Onion.WebApi.Controllers
{
    [ApiController]
    [ProducesResponseType(400, Type = typeof(ErrorRes))]
    [ProducesResponseType(401, Type = typeof(ErrorRes))]
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
        public async Task<ActionResult<ItemRes>> Get([FromRoute] Guid itemId)
        {
            return StatusCode(200, await _itemService.GetAsync(itemId));
        }

        //[ProducesResponseType(200)]
        //[HttpGet]
        //public async Task<ActionResult<IList<ItemRes>>> List()
        //{
        //    return StatusCode(200, await _itemService.ListAsync());
        //}

        [ProducesResponseType(200)]
        [HttpGet]
        public async Task<ActionResult<PaginableList<ItemRes>>> Paginate(
            [FromQuery][Range(1, 20)] int size,
            [FromQuery][Range(1, int.MaxValue)] int page)
        {
            return StatusCode(200, await _itemService.PaginateAsync(size, page));
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
        public async Task<ActionResult<ItemRes>> Delete([FromRoute] Guid itemId)
        {
            return StatusCode(200, await _itemService.DeleteAsync(itemId));
        }

        [ProducesResponseType(200)]
        [ProducesResponseType(404, Type = typeof(ErrorRes))]
        [HttpPut("{itemId}")]
        public async Task<ActionResult<ItemRes>> Update([FromRoute] Guid itemId, [FromBody] ItemReq body)
        {
            return StatusCode(200, await _itemService.UpdateAsync(itemId, body));
        }

        [Authorize(Roles = "Admin")]
        [ProducesResponseType(200)]
        [HttpGet("foo")]
        public async Task<ActionResult<bool>> Foo()
        {
            return StatusCode(200, await _itemService.FooAsync());
        }
    }
}

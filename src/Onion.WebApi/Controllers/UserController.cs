using Microsoft.AspNetCore.Mvc;
using Onion.Application.Services.Abstractions;
using Onion.Application.Services.Models.Item;
using Onion.Application.Services.Models.User;
using Onion.WebApi.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Onion.WebApi.Controllers
{
    [ApiController]
    [ProducesResponseType(400, Type = typeof(ErrorRes))]
    [ProducesResponseType(500, Type = typeof(ErrorRes))]
    [Produces("application/json")]
    [Route("api/users")]
    public class UserController : BaseController
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        [HttpGet("{userId}")]
        public async Task<ActionResult<UserRes>> Get([FromRoute] Guid userId)
        {
            return StatusCode(200, await _userService.GetAsync(userId));
        }

        [ProducesResponseType(200)]
        [HttpGet]
        public async Task<ActionResult<IList<UserRes>>> List()
        {
            return StatusCode(200, await _userService.ListAsync());
        }

        [ProducesResponseType(201)]
        [HttpPost]
        public async Task<ActionResult<UserRes>> Create([FromBody] UserReq body)
        {
            return StatusCode(201, await _userService.CreateAsync(body));
        }

    }
}

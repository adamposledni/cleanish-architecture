using Microsoft.AspNetCore.Mvc;
using Onion.Application.Services.Abstractions;
using Onion.Application.Services.Models.Item;
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
        [HttpGet("foo")]
        public async Task<ActionResult<bool>> Foo()
        {
            return StatusCode(200, await _userService.FooAsync());
        }
    }
}

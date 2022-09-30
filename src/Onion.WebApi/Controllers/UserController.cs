using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Onion.Application.Services.Users;
using Onion.Application.Services.Users.Models;
using Onion.WebApi.Atributes;
using Onion.WebApi.Models;

namespace Onion.WebApi.Controllers;

[ApiController]
[ProducesErrorResponse(400)]
[ProducesErrorResponse(500)]
[Produces("application/json")]
[ApiRoute("users")]
public class UserController : BaseController
{
    private readonly IUserService _userService;

    public UserController(IUserService userService)
    {
        _userService = userService;
    }

    [Authorize]
    [ProducesResponseType(200)]
    [ProducesErrorResponse(404)]
    [HttpGet("{userId}")]
    public async Task<ActionResult<UserRes>> Get([FromRoute] Guid userId)
    {
        return StatusCode(200, await _userService.GetAsync(userId));
    }

    [Authorize]
    [ProducesResponseType(200)]
    [HttpGet]
    public async Task<ActionResult<IEnumerable<UserRes>>> List()
    {
        return StatusCode(200, await _userService.ListAsync());
    }

    [ProducesResponseType(201)]
    [HttpPost]
    public async Task<ActionResult<UserRes>> Create([FromBody] UserReq body)
    {
        return StatusCode(201, await _userService.CreateAsync(body));
    }

    [ProducesResponseType(200)]
    [ProducesErrorResponse(404)]
    [HttpGet("foo")]
    public async Task<ActionResult<Foo1Res>> Foo()
    {
        return StatusCode(200, await _userService.FooAsync());
    }
}

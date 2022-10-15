using Microsoft.AspNetCore.Mvc;
using Onion.App.Logic.Users.Models;
using Onion.App.Logic.Users.UseCases;
using Onion.Pres.WebApi.Atributes;

namespace Onion.Pres.WebApi.Controllers;

[ApiController]
[ProducesErrorResponse(400)]
[ProducesErrorResponse(500)]
[Produces("application/json")]
[ApiRoute("users")]

// TODO: presentation contracts vs. business contracts
public class UserController : BaseController
{
    [ProducesResponseType(200)]
    [ProducesErrorResponse(404)]
    [HttpGet("{userId}")]
    public async Task<ActionResult<UserRes>> Get([FromRoute] Guid userId)
    {
        return StatusCode(200, await Mediator.Send(new GetUserRequest(userId)));
    }

    [ProducesResponseType(200)]
    [HttpGet]
    public async Task<ActionResult<IEnumerable<UserRes>>> List()
    {
        return StatusCode(200, await Mediator.Send(new ListUsersRequest()));
    }

    [ProducesResponseType(201)]
    [HttpPost]
    public async Task<ActionResult<UserRes>> Create(CreateUserRequest body)
    {
        return StatusCode(201, await Mediator.Send(body));
    }

    [ProducesResponseType(200)]
    [ProducesErrorResponse(404)]
    [HttpGet("foo")]
    public async Task<ActionResult<Foo1Res>> Foo()
    {
        return StatusCode(200, await Mediator.Send(new FooRequest()));
    }
}

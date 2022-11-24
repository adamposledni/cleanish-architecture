using Microsoft.AspNetCore.Mvc;
using Onion.App.Logic.Users.Models;
using Onion.App.Logic.Users.UseCases;
using Onion.Pres.WebApi.Atributes;

namespace Onion.Pres.WebApi.Controllers;

[ApiRoute("users")]
public class UserController : BaseController
{
    [ProducesResponseType(200)]
    [ProducesErrorResponse(404)]
    [HttpGet("{userId}")]
    public async Task<ActionResult<UserRes>> Get([FromRoute] Guid userId)
    {
        return Ok(await Mediate(new GetUserRequest(userId)));
    }

    [ProducesResponseType(200)]
    [HttpGet]
    public async Task<ActionResult<IEnumerable<UserRes>>> List()
    {
        return Ok(await Mediate(new ListUsersRequest()));
    }

    [ProducesResponseType(201)]
    [HttpPost]
    public async Task<ActionResult<UserRes>> Create([FromBody] CreateUserRequest body)
    {
        return Created(await Mediate(body));
    }

    [ProducesResponseType(204)]
    [ProducesErrorResponse(404)]
    [HttpDelete("{userId}")]
    public async Task<ActionResult> Delete([FromRoute] Guid userId)
    {
        return NoContent(await Mediate(new DeleteUserRequest(userId)));
    }

    [ProducesResponseType(200)]
    [HttpGet("foo")]
    public async Task<ActionResult<Foo1Res>> Foo()
    {
        return Ok(await Mediate(new FooRequest()));
    }
}

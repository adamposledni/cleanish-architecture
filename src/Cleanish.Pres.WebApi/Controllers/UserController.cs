using Microsoft.AspNetCore.Mvc;
using Cleanish.App.Logic.Users.Models;
using Cleanish.App.Logic.Users.UseCases;
using Cleanish.Pres.WebApi.Atributes;

namespace Cleanish.Pres.WebApi.Controllers;

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
}

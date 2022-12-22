using Microsoft.AspNetCore.Mvc;
using Cleanish.App.Logic.Auth.Models;
using Cleanish.App.Logic.Auth.UseCases;
using Cleanish.Pres.WebApi.Atributes;

namespace Cleanish.Pres.WebApi.Controllers;

[ApiRoute("auth")]
public class AuthController : BaseApiController
{
    [ProducesResponseType(200)]
    [HttpPost("login")]
    public async Task<ActionResult<AuthRes>> Login(UserBasicLoginRequest body)
    {
        return Ok(await Mediate(body));
    }
}

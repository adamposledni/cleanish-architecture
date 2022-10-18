using Microsoft.AspNetCore.Mvc;
using Onion.App.Logic.Auth.Models;
using Onion.App.Logic.Auth.UseCases;
using Onion.Pres.WebApi.Atributes;

namespace Onion.Pres.WebApi.Controllers;

[ApiRoute("auth")]
public class AuthController : BaseController
{
    [ProducesResponseType(200)]
    [HttpPost("login")]
    public async Task<ActionResult<AuthRes>> Login(UserBasicLoginRequest body)
    {
        return Ok(await Mediator.Send(body));
    }

    [ProducesResponseType(200)]
    [HttpPost("google-login")]
    public async Task<ActionResult<AuthRes>> GoogleLogin(UserGoogleLoginRequest body)
    {
        return Ok(await Mediator.Send(body));
    }

    [ProducesResponseType(200)]
    [ProducesErrorResponse(404)]
    [HttpPost("revoke-refresh-token")]
    public async Task<ActionResult<RefreshTokenRes>> RevokeRefreshToken(RevokeRefreshTokenRequest body)
    {
        return Ok(await Mediator.Send(body));
    }

    [ProducesResponseType(200)]
    [ProducesErrorResponse(404)]
    [HttpPost("refresh-access-token")]
    public async Task<ActionResult<AuthRes>> RefreshAccessToken(RefreshAccessTokenRequest body)
    {
        return Ok(await Mediator.Send(body));
    }
}

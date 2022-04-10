using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Onion.Application.Services.Auth;
using Onion.Application.Services.Auth.Models;
using Onion.WebApi.Atributes;
using Onion.WebApi.Models;

namespace Onion.WebApi.Controllers;

[ApiController]
[ProducesErrorResponse(400)]
[ProducesErrorResponse(500)]
[Produces("application/json")]
[ApiRoute("auth")]
public class AuthController : BaseController
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [AllowAnonymous]
    [ProducesResponseType(200)]
    [HttpPost("login")]
    public async Task<ActionResult<AuthRes>> Login([FromBody] PasswordAuthReq body)
    {
        return StatusCode(200, await _authService.LoginAsync(body));
    }

    [AllowAnonymous]
    [ProducesResponseType(200)]
    [HttpPost("google-login")]
    public async Task<ActionResult<AuthRes>> GoogleLogin([FromBody] IdTokenAuthReq body)
    {
        return StatusCode(200, await _authService.GoogleLoginAsync(body));
    }

    [Authorize]
    [ProducesResponseType(200)]
    [ProducesErrorResponse(404)]
    [HttpPost("revoke-refresh-token")]
    public async Task<ActionResult<RefreshTokenRes>> RevokeRefreshToken([FromBody] RefreshTokenReq body)
    {
        return StatusCode(200, await _authService.RevokeRefreshTokenAsync(body));
    }

    [AllowAnonymous]
    [ProducesResponseType(200)]
    [ProducesErrorResponse(404)]
    [HttpPost("refresh-access-token")]
    public async Task<ActionResult<AuthRes>> RefreshAccessToken([FromBody] RefreshTokenReq body)
    {
        return StatusCode(200, await _authService.RefreshAccessTokenAsync(body));
    }
}

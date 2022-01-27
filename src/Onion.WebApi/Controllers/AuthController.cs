using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Onion.Application.Services.Auth;
using Onion.Application.Services.Auth.Models;
using Onion.WebApi.Models;

namespace Onion.WebApi.Controllers;

[ApiController]
[ProducesResponseType(400, Type = typeof(ErrorRes))]
[ProducesResponseType(500, Type = typeof(ErrorRes))]
[Produces("application/json")]
[Route("api/auth")]
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
    [ProducesResponseType(404, Type = typeof(ErrorRes))]
    [ProducesResponseType(200)]
    [HttpPost("revoke-refresh-token")]
    public async Task<ActionResult<RefreshTokenRes>> RevokeRefreshToken([FromBody] RefreshTokenReq body)
    {
        return StatusCode(200, await _authService.RevokeRefreshTokenAsync(body.RefreshToken));
    }

    [AllowAnonymous]
    [ProducesResponseType(200)]
    [ProducesResponseType(404, Type = typeof(ErrorRes))]
    [HttpPost("refresh-access-token")]
    public async Task<ActionResult<AuthRes>> RefreshAccessToken([FromBody] RefreshTokenReq body)
    {
        return StatusCode(200, await _authService.RefreshAccessTokenAsync(body.RefreshToken));
    }
}

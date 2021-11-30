using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Onion.Application.Services.Abstractions;
using Onion.Application.Services.Models.Auth;
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

        [AllowAnonymous]
        [ProducesResponseType(200)]
        [HttpPost("facebook-login")]
        public async Task<ActionResult<AuthRes>> FacebookLogin([FromBody] IdTokenAuthReq body)
        {
            return StatusCode(200, await _authService.FacebookLoginAsync(body));
        }
    }
}

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Onion.Application.Services.Auth;
using Onion.Application.Services.Auth.Models;
using Onion.WebApi.Models;
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
    }
}

using Onion.Application.Services.Auth.Models;

namespace Onion.Application.Services.Auth;

public interface IAuthService
{
    Task<AuthRes> LoginAsync(PasswordAuthReq model);
    Task<AuthRes> GoogleLoginAsync(IdTokenAuthReq model);
    Task<AuthRes> RefreshAccessTokenAsync(string refreshToken);
    Task<RefreshTokenRes> RevokeRefreshTokenAsync(string refreshToken);

}

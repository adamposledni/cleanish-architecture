using Onion.App.Logic.Auth.Models;

namespace Onion.App.Logic.Auth;

public interface IAuthService
{
    Task<AuthRes> LoginAsync(PasswordAuthReq model);
    Task<AuthRes> GoogleLoginAsync(IdTokenAuthReq model);
    Task<AuthRes> RefreshAccessTokenAsync(RefreshTokenReq model);
    Task<RefreshTokenRes> RevokeRefreshTokenAsync(RefreshTokenReq model);

}

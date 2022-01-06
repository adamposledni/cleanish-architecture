using Onion.Application.Services.Auth.Models;
using System.Threading.Tasks;

namespace Onion.Application.Services.Auth
{
    public interface IAuthService
    {
        Task<AuthRes> LoginAsync(PasswordAuthReq model);
        Task<AuthRes> GoogleLoginAsync(IdTokenAuthReq model);
    }
}

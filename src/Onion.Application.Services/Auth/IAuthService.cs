using Onion.Application.Services.Auth.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Onion.Application.Services.Auth
{
    public interface IAuthService
    {
        Task<AuthRes> LoginAsync(PasswordAuthReq model);
        Task<AuthRes> GoogleLoginAsync(IdTokenAuthReq model);
        Task<AuthRes> FacebookLoginAsync(IdTokenAuthReq model);
    }
}

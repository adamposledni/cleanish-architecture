using Onion.Application.Services.Models.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Onion.Application.Services.Abstractions
{
    public interface IAuthService
    {
        Task<AuthRes> LoginAsync(PasswordAuthReq model);
        Task<AuthRes> GoogleLoginAsync(IdTokenAuthReq model);
        Task<AuthRes> FacebookLoginAsync(IdTokenAuthReq model);
    }
}

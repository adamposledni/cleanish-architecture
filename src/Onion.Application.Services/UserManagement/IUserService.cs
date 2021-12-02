using Onion.Application.Services.UserManagement.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Onion.Application.Services.UserManagement
{
    public interface IUserService
    {
        Task<UserRes> GetAsync(Guid userId);
        Task<IList<UserRes>> ListAsync();
        Task<UserRes> CreateAsync(UserReq model);
    }
}

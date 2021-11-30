using Onion.Application.Services.Models.Item;
using Onion.Application.Services.Models.User;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Onion.Application.Services.Abstractions
{
    public interface IUserService
    {
        Task<UserRes> GetAsync(Guid userId);
        Task<IList<UserRes>> ListAsync();
        Task<UserRes> CreateAsync(UserReq model);
    }
}

using Onion.Application.Services.Users.Models;

namespace Onion.Application.Services.Users;

public interface IUserService
{
    Task<UserRes> GetAsync(Guid userId);
    Task<IEnumerable<UserRes>> ListAsync();
    Task<UserRes> CreateAsync(UserReq model);
}

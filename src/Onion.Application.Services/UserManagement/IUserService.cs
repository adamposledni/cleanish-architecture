using Onion.Application.Services.UserManagement.Models;

namespace Onion.Application.Services.UserManagement;

public interface IUserService
{
    Task<UserRes> GetAsync(Guid userId);
    Task<IEnumerable<UserRes>> ListAsync();
    Task<UserRes> CreateAsync(UserReq model);
}

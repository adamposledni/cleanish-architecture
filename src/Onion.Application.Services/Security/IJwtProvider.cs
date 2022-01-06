using Onion.Application.DataAccess.Entities;

namespace Onion.Application.Services.Security
{
    public interface IJwtProvider
    {
        string GenerateJwt(User user);
    }
}

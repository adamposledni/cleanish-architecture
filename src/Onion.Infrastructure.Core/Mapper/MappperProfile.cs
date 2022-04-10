using Onion.Application.DataAccess.Database.Entities;
using Onion.Application.Services.Auth.Models;
using Onion.Application.Services.Users.Models;
using AM = AutoMapper;

namespace Onion.Infrastructure.Core.Mapper;

public static class MappperProfile
{
    public static void Configure(AM.IMapperConfigurationExpression configurationExpression)
    {
        configurationExpression.CreateMap<User, UserRes>();
        configurationExpression.CreateMap<UserReq, User>();

        configurationExpression.CreateMap<User, AuthRes>();

        configurationExpression.CreateMap<RefreshToken, RefreshTokenRes>();
    }
}

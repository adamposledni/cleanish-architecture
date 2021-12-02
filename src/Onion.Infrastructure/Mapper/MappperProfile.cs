using Onion.Application.DataAccess.Entities;
using Onion.Application.Services.Auth.Models;
using Onion.Application.Services.ItemManagement.Models;
using Onion.Application.Services.UserManagement.Models;
using AM = AutoMapper;

namespace Onion.Infrastructure.Mapper
{
    public static class MappperProfile
    {
        public static void Configure(AM.IMapperConfigurationExpression configurationExpression)
        {
            configurationExpression.CreateMap<Item, ItemRes>();
            configurationExpression.CreateMap<ItemReq, Item>();

            configurationExpression.CreateMap<User, UserRes>();
            configurationExpression.CreateMap<UserReq, User>();

            configurationExpression.CreateMap<User, AuthRes>();
        }
    }
}

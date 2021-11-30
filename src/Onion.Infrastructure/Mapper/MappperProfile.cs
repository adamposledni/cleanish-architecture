using Onion.Application.DataAccess.Entities;
using Onion.Application.Services.Models.Auth;
using Onion.Application.Services.Models.Item;
using Onion.Application.Services.Models.User;
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

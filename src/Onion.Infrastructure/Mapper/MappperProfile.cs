using Onion.Application.Domain.Entities;
using Onion.Application.Services.Models.Item;
using AM = AutoMapper;

namespace Onion.Infrastructure.Mapper
{
    public static class MappperProfile
    {
        public static void Configure(AM.IMapperConfigurationExpression configurationExpression)
        {
            configurationExpression.CreateMap<Item, ItemRes>();
            configurationExpression.CreateMap<ItemReq, Item>();
        }
    }
}

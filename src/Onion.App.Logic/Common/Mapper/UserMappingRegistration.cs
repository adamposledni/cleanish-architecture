using Mapster;
using Onion.App.Data.Database.Entities;
using Onion.App.Logic.Users.Models;

namespace Onion.App.Logic.Common.Mapper;

public class UserMappingRegistration : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<User, Foo1Res>()
            .Map(dest => dest.FooString, src => $"Email is {src.Email}");
    }
}

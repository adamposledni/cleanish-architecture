using Onion.Application.DataAccess.Database.Entities;
using Onion.Application.Services.Auth.Models;
using Onion.Application.Services.TodoLists.Models;
using Onion.Application.Services.Users.Models;
using AM = AutoMapper;

namespace Onion.Infrastructure.Core.Mapper;

public static class MapperProfile
{
    public static void Configure(AM.IMapperConfigurationExpression configuration)
    {
        // source -> destination

        configuration.CreateMap<User, UserRes>();
        configuration.CreateMap<UserReq, User>();

        configuration.CreateMap<User, AuthRes>();

        configuration.CreateMap<RefreshToken, RefreshTokenRes>();

        configuration.CreateMap<TodoItem, TodoItemRes>();
        configuration.CreateMap<TodoItemReq, TodoItem>();

        configuration.CreateMap<TodoListReq, TodoList>();
        configuration.CreateMap<TodoList, TodoListRes>();
        configuration.CreateMap<TodoList, TodoListBriefRes>();

        configuration.CreateMap<User, Foo1Res>();
        configuration.CreateMap<TodoList, Foo2Res>();
        configuration.CreateMap<TodoItem, Foo3Res>();
    }
}

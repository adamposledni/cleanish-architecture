using Mapster;
using Cleanish.App.Data.Database.Entities;
using Cleanish.App.Logic.TodoLists.Models;

namespace Cleanish.App.Logic.TodoItems.Models.Mapping;

internal class TodoItemMappingRegistration : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<TodoItem, TodoItemRes>()
            .Map(dest => dest.Name, src => src.Title);
    }
}
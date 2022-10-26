using Microsoft.AspNetCore.Mvc;
using Onion.App.Logic.TodoItems.UseCases;
using Onion.App.Logic.TodoLists.Models;
using Onion.Pres.WebApi.Atributes;

namespace Onion.Pres.WebApi.Controllers;

[ApiRoute("todo-lists")]
internal class TodoItemController : BaseController
{
    [ProducesResponseType(200)]
    [ProducesErrorResponse(404)]
    [HttpGet("{todoListId}/items/{todoItemId}")]
    public async Task<ActionResult<TodoItemRes>> Get([FromRoute] Guid todoListId, [FromRoute] Guid todoItemId)
    {
        return Ok(await Mediate(new GetTodoItemRequest(todoListId, todoItemId)));
    }

    [ProducesResponseType(200)]
    [HttpGet("{todoListId}/items")]
    public async Task<ActionResult<IEnumerable<TodoItemRes>>> List(Guid todoListId)
    {
        return Ok(await Mediate(new ListTodoItemsRequest(todoListId)));
    }

    [ProducesResponseType(201)]
    [HttpPost("{todoListId}/items")]
    public async Task<ActionResult<TodoItemRes>> Create(Guid todoListId, CreateTodoItemRequest body)
    {
        ValidateEquals(todoListId, body.TodoListId);

        return Created(await Mediate(new CreateTodoItemRequest()));
    }

    [ProducesResponseType(200)]
    [HttpPut("{todoListId}/items/{todoItemId}")]
    public async Task<ActionResult<TodoItemRes>> Update(
        Guid todoListId,
        Guid todoItemId,
        UpdateTodoItemRequest body)
    {
        ValidateEquals(todoListId, body.TodoListId);
        ValidateEquals(todoItemId, body.Id);

        return Ok(await Mediate(body));
    }
}
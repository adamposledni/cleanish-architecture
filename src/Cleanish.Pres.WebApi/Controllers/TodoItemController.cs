using Microsoft.AspNetCore.Mvc;
using Cleanish.App.Logic.TodoItems.UseCases;
using Cleanish.App.Logic.TodoLists.Models;
using Cleanish.Pres.WebApi.Atributes;

namespace Cleanish.Pres.WebApi.Controllers;

[ApiRoute("todo-items")]
public class TodoItemController : BaseController
{
    [ProducesResponseType(200)]
    [ProducesErrorResponse(404)]
    [HttpGet("{todoItemId}")]
    public async Task<ActionResult<TodoItemRes>> Get([FromRoute] Guid todoItemId)
    {
        return Ok(await Mediate(new GetTodoItemRequest(todoItemId)));
    }

    [ProducesResponseType(200)]
    [HttpGet]
    public async Task<ActionResult<IEnumerable<TodoItemRes>>> List()
    {
        return Ok(await Mediate(new ListTodoItemsRequest()));
    }

    [ProducesResponseType(201)]
    [HttpPost]
    public async Task<ActionResult<TodoItemRes>> Create(CreateTodoItemRequest body)
    {
        return Created(await Mediate(body));
    }

    [ProducesResponseType(200)]
    [HttpPut("{todoItemId}")]
    public async Task<ActionResult<TodoItemRes>> Update(
        Guid todoItemId,
        UpdateTodoItemRequest body)
    {
        ValidateEquals(todoItemId, body.Id);

        return Ok(await Mediate(body));
    }

    [ProducesResponseType(204)]
    [ProducesErrorResponse(404)]
    [HttpDelete("{todoItemId}")]
    public async Task<ActionResult> Delete(Guid todoItemId)
    {
        return NoContent(await Mediate(new DeleteTodoItemRequest(todoItemId)));
    }
}
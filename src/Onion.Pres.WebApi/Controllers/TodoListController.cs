using Microsoft.AspNetCore.Mvc;
using Onion.App.Logic.TodoLists.Models;
using Onion.App.Logic.TodoLists.UseCases;
using Onion.Pres.WebApi.Atributes;

namespace Onion.Pres.WebApi.Controllers;

[ApiRoute("todo-lists")]
public class TodoListController : BaseController
{
    [ProducesResponseType(200)]
    [ProducesErrorResponse(404)]
    [HttpGet("{todoListId}")]
    public async Task<ActionResult<TodoListRes>> Get(Guid todoListId)
    {
        return Ok(await Mediator.Send(new GetTodoListRequest(todoListId)));
    }

    [ProducesResponseType(200)]
    [HttpGet]
    public async Task<ActionResult<IEnumerable<TodoListBriefRes>>> List()
    {
        return Ok(await Mediator.Send(new ListTodoListsRequest()));
    }

    [ProducesResponseType(201)]
    [HttpPost]
    public async Task<ActionResult<TodoListRes>> Create(CreateTodoListRequest body)
    {
        return Created(await Mediator.Send(body));
    }
}

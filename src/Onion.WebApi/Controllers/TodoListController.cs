using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Onion.Application.Services.TodoLists;
using Onion.Application.Services.TodoLists.Models;
using Onion.WebApi.Atributes;

namespace Onion.WebApi.Controllers;

[Authorize]
[ApiController]
[ProducesErrorResponse(400)]
[ProducesErrorResponse(500)]
[Produces("application/json")]
[ApiRoute("todo-lists")]
public class TodoListController : BaseController
{
    private readonly ITodoListService _todoListService;
    private readonly ITodoItemService _todoItemService;

    public TodoListController(ITodoListService todoListService, ITodoItemService todoItemService)
    {
        _todoListService = todoListService;
        _todoItemService = todoItemService;
    }

    
    [ProducesResponseType(200)]
    [ProducesErrorResponse(404)]
    [HttpGet("{todoListId}")]
    public async Task<ActionResult<TodoListRes>> Get([FromRoute] Guid todoListId)
    {
        return StatusCode(200, await _todoListService.GetAsync(todoListId));
    }

    [ProducesResponseType(200)]
    [HttpGet]
    public async Task<ActionResult<IEnumerable<TodoListBriefRes>>> List()
    {
        return StatusCode(200, await _todoListService.ListAsync());
    }

    [ProducesResponseType(201)]
    [HttpPost]
    public async Task<ActionResult<TodoListRes>> Create([FromBody] TodoListReq body)
    {
        return StatusCode(201, await _todoListService.CreateAsync(body));
    }

    [ProducesResponseType(200)]
    [ProducesErrorResponse(404)]
    [HttpGet("{todoListId}/items/{todoItemId}")]
    public async Task<ActionResult<TodoListRes>> GetTodoItem([FromRoute] Guid todoListId, [FromRoute] Guid todoItemId)
    {
        return StatusCode(200, await _todoItemService.GetAsync(todoItemId, todoListId));
    }

    [ProducesResponseType(200)]
    [HttpGet("{todoListId}/items")]
    public async Task<ActionResult<IEnumerable<TodoListBriefRes>>> ListTodoItems([FromRoute] Guid todoListId)
    {
        return StatusCode(200, await _todoItemService.ListAsync(todoListId));
    }

    [ProducesResponseType(201)]
    [HttpPost("{todoListId}/items")]
    public async Task<ActionResult<TodoListRes>> CreateTodoItem([FromRoute] Guid todoListId, [FromBody] TodoItemReq body)
    {
        return StatusCode(201, await _todoItemService.CreateAsync(body));
    }
}

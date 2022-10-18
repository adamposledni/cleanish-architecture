using FluentValidation;
using MediatR;
using Onion.App.Data.Database.Entities;
using Onion.App.Data.Database.Repositories;
using Onion.App.Logic.Common.Attributes;
using Onion.App.Logic.Common.Security;
using Onion.App.Logic.TodoLists.Models;
using Onion.Shared.Mapper;
using System.Threading;

namespace Onion.App.Logic.TodoLists.UseCases;

[Authorize]
public class CreateTodoListRequest : IRequest<TodoListRes>
{
    public string Title { get; set; }
}

internal class CreateTodoListRequestValidator : AbstractValidator<CreateTodoListRequest>
{
    public CreateTodoListRequestValidator()
    {
        RuleFor(x => x.Title).NotEmpty();
    }
}

internal class CreateTodoListRequestHandler : IRequestHandler<CreateTodoListRequest, TodoListRes>
{
    private readonly ISecurityContextProvider _securityContextProvider;
    private readonly IObjectMapper _mapper;
    private readonly ITodoListRepository _todoListRepository;

    public CreateTodoListRequestHandler(
        ISecurityContextProvider securityContextProvider,
        ITodoListRepository todoListRepository,
        IObjectMapper mapper)
    {
        _securityContextProvider = securityContextProvider;
        _mapper = mapper;
        _todoListRepository = todoListRepository;
    }

    public async Task<TodoListRes> Handle(CreateTodoListRequest request, CancellationToken cancellationToken)
    {
        var subjectId = _securityContextProvider.GetSubjectId();

        var newTodoList = _mapper.Map<TodoList>(request, tl => tl.UserId = subjectId);
        newTodoList = await _todoListRepository.CreateAsync(newTodoList);

        return _mapper.Map<TodoListRes>(newTodoList);
    }
}

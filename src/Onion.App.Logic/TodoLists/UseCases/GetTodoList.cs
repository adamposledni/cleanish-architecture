using FluentValidation;
using MediatR;
using Onion.App.Data.Cache;
using Onion.App.Data.Database.Repositories;
using Onion.App.Logic.Common.Attributes;
using Onion.App.Logic.Common.Security;
using Onion.App.Logic.TodoLists.Exceptions;
using Onion.App.Logic.TodoLists.Models;
using Onion.Shared.Exceptions;
using Onion.Shared.Mapper;
using System.Threading;

namespace Onion.App.Logic.TodoLists.UseCases;

[Authorize]
public class GetTodoListRequest : IRequest<TodoListRes>
{
    public Guid Id { get; set; }

    public GetTodoListRequest(Guid id)
    {
        Id = id;
    }
}

internal class GetTodoListRequestValidator : AbstractValidator<GetTodoListRequest>
{
    public GetTodoListRequestValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}

public class GetTodoListRequestHandler : IRequestHandler<GetTodoListRequest, TodoListRes>
{
    private readonly ISecurityContextProvider _securityContextProvider;
    private readonly IObjectMapper _mapper;
    private readonly ITodoListRepository _cachedTodoListRepository;

    public GetTodoListRequestHandler(
        ISecurityContextProvider securityContextProvider,
        Cached<ITodoListRepository> cachedTodoListRepository,
        IObjectMapper mapper)
    {
        _securityContextProvider = securityContextProvider;
        _mapper = mapper;
        _cachedTodoListRepository = cachedTodoListRepository.Value;
    }

    public async Task<TodoListRes> Handle(GetTodoListRequest request, CancellationToken cancellationToken)
    {
        var subjectId = _securityContextProvider.GetSubjectId();

        var todoList = await _cachedTodoListRepository.GetByIdAsync(request.Id);
        if (todoList == null) throw new TodoListNotFoundException();
        if (todoList.UserId != subjectId) throw new NotAuthorizedException();

        return _mapper.Map<TodoListRes>(todoList);
    }
}

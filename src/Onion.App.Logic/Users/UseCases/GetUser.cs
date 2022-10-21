using FluentValidation;
using MediatR;
using Onion.App.Data.Cache;
using Onion.App.Data.Database.Repositories;
using Onion.App.Logic.Common.Attributes;
using Onion.App.Logic.Users.Exceptions;
using Onion.App.Logic.Users.Models;
using Onion.Shared.Mapper;
using System.Threading;

namespace Onion.App.Logic.Users.UseCases;

[Authorize]
public class GetUserRequest : IRequest<UserRes>
{
    public Guid UserId { get; set; }

    public GetUserRequest(Guid userId)
    {
        UserId = userId;
    }
}

internal class GetUserRequestValidator : AbstractValidator<GetUserRequest>
{
    public GetUserRequestValidator()
    {
        RuleFor(x => x.UserId).NotEmpty();
    }
}

public class GetUserRequestHandler : IRequestHandler<GetUserRequest, UserRes>
{
    private readonly IUserRepository _userRepository;
    private readonly IObjectMapper _mapper;

    public GetUserRequestHandler(Cached<IUserRepository> userRepository, IObjectMapper mapper)
    {
        _userRepository = userRepository.Value;
        _mapper = mapper;
    }

    public async Task<UserRes> Handle(GetUserRequest request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByIdAsync(request.UserId);
        if (user == null) throw new UserNotFoundException();

        return _mapper.Map<UserRes>(user);
    }
}

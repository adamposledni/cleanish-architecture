using FluentValidation;
using MediatR;
using Onion.App.Data.Cache;
using Onion.App.Data.Database.Repositories;
using Onion.App.Logic.Users.Models;
using Onion.Shared.Mapper;
using System.Threading;

namespace Onion.App.Logic.Users.UseCases;

public class FooRequest : IRequest<Foo1Res>
{
}

internal class FooRequestValidator : AbstractValidator<FooRequest>
{
}

internal class FooRequestHandler : IRequestHandler<FooRequest, Foo1Res>
{
    private readonly IUserRepository _userRepository;
    private readonly IObjectMapper _mapper;

    public FooRequestHandler(Cached<IUserRepository> userRepository, IObjectMapper mapper)
    {
        _userRepository = userRepository.Value;
        _mapper = mapper;
    }

    public async Task<Foo1Res> Handle(FooRequest request, CancellationToken cancellationToken)
    {
        var foo = await _userRepository.FooAsync();
        return _mapper.Map<Foo1Res>(foo, (d) => d.Foo = 1);
    }
}

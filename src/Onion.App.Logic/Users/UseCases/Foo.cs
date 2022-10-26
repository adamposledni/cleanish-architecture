using FluentValidation;
using MediatR;
using Onion.App.Data.Cache;
using Onion.App.Data.Database.Repositories;
using Onion.App.Logic.Users.Models;
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

    public FooRequestHandler(Cached<IUserRepository> userRepository)
    {
        _userRepository = userRepository.Value;
    }

    public async Task<Foo1Res> Handle(FooRequest request, CancellationToken cancellationToken)
    {
        var foo = await _userRepository.FooAsync();
        return foo.Adapt<Foo1Res>(f =>
        {
            f.Foo = 1;
        });
    }
}

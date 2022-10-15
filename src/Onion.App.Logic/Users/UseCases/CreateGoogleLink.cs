using FluentValidation;
using MediatR;
using Onion.App.Data.Database.Repositories;
using Onion.App.Data.Security;
using Onion.App.Logic.Auth.Exceptions;
using Onion.App.Logic.Common.Attributes;
using Onion.App.Logic.Security;
using Onion.App.Logic.Users.Models;
using Onion.Shared.Helpers;
using Onion.Shared.Mapper;
using System.Threading;

namespace Onion.App.Logic.Users.UseCases;

[Authorize]
public class CreateGoogleLinkRequest : IRequest<UserRes>
{
    public string IdToken { get; set; }
}

internal class CreateGoogleLinkRequestValidator: AbstractValidator<CreateGoogleLinkRequest>
{
    public CreateGoogleLinkRequestValidator()
    {
        RuleFor(x => x.IdToken).NotEmpty();
    }
}

internal class CreateGoogleLinkRequestHandler : IRequestHandler<CreateGoogleLinkRequest, UserRes>
{
    private readonly IUserRepository _userRepository;
    private readonly IObjectMapper _mapper;
    private readonly ISecurityContextProvider _securityContextProvider;
    private readonly IGoogleAuthProvider _googleAuthProvider;

    public CreateGoogleLinkRequestHandler(
        IUserRepository userRepository, 
        IObjectMapper mapper, 
        ISecurityContextProvider securityContextProvider,
        IGoogleAuthProvider googleAuthProvider)
    {
        _userRepository = userRepository;
        _mapper = mapper;
        _securityContextProvider = securityContextProvider;
        _googleAuthProvider = googleAuthProvider;
    }

    public async Task<UserRes> Handle(CreateGoogleLinkRequest request, CancellationToken cancellationToken)
    {
        var securityContext = _securityContextProvider.SecurityContext;
        Guard.NotNull(securityContext, nameof(securityContext));

        var googleIdentity = await _googleAuthProvider.GetIdentityAsync(request.IdToken);
        if (googleIdentity == null) throw new InvalidGoogleIdTokenException();

        var user = await _userRepository.GetByIdAsync(securityContext.SubjectId);
        Guard.NotNull(user, nameof(user));

        if (!string.IsNullOrWhiteSpace(user.GoogleSubjectId)) throw new GoogleLinkAlreadyExistsException();

        user.GoogleSubjectId = googleIdentity.SubjectId;
        await _userRepository.UpdateAsync(user);

        return _mapper.Map<UserRes>(user);
    }
}

using FluentValidation;
using MediatR;
using Microsoft.Extensions.Options;
using Onion.App.Data.Database.Repositories;
using Onion.App.Data.Security;
using Onion.App.Logic.Auth.Exceptions;
using Onion.App.Logic.Auth.Models;
using Onion.App.Logic.Common;
using Onion.Shared.Clock;
using Onion.Shared.Mapper;
using System.Threading;

namespace Onion.App.Logic.Auth.UseCases;

public class UserGoogleLoginRequest : IRequest<AuthRes>
{
    public string IdToken { get; set; }
}

internal class UserGoogleLoginRequestValidator : AbstractValidator<UserGoogleLoginRequest>
{
    public UserGoogleLoginRequestValidator()
    {
        RuleFor(x => x.IdToken).NotEmpty();
    }
}

public class UserGoogleLoginHandler : IRequestHandler<UserGoogleLoginRequest, AuthRes>
{
    private readonly IUserRepository _userRepository;
    private readonly ICryptographyService _cryptographyService;
    private readonly IClockProvider _clockProvider;
    private readonly IWebTokenService _webTokenService;
    private readonly IRefreshTokenRepository _refreshTokenRepository;
    private readonly IGoogleAuthProvider _googleAuthProvider;
    private readonly ApplicationSettings _applicationSettings;
    private readonly IObjectMapper _mapper;

    public UserGoogleLoginHandler(
        IUserRepository userRepository,
        ICryptographyService cryptographyService,
        IClockProvider clockProvider,
        IWebTokenService webTokenService,
        IRefreshTokenRepository refreshTokenRepository,
        IGoogleAuthProvider googleAuthProvider,
        IOptions<ApplicationSettings> applicationSettings,
        IObjectMapper mapper)
    {
        _userRepository = userRepository;
        _cryptographyService = cryptographyService;
        _clockProvider = clockProvider;
        _webTokenService = webTokenService;
        _refreshTokenRepository = refreshTokenRepository;
        _googleAuthProvider = googleAuthProvider;
        _applicationSettings = applicationSettings.Value;
        _mapper = mapper;
    }

    public async Task<AuthRes> Handle(UserGoogleLoginRequest request, CancellationToken cancellationToken)
    {
        var googleIdentity = await _googleAuthProvider.GetIdentityAsync(request.IdToken);
        if (googleIdentity == null) throw new InvalidGoogleIdTokenException();

        var user = await _userRepository.GetByGoogleIdAsync(googleIdentity.SubjectId);
        if (user == null) throw new GoogleLinkMissingException();

        return await AuthCommonLogic.IssueAccessAsync(
            _refreshTokenRepository,
            _webTokenService,
            _cryptographyService,
            _clockProvider,
            _mapper,
            user,
            _applicationSettings.AccessTokenLifetime,
            _applicationSettings.RefreshTokenLifetime);
    }
}
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

public class RefreshAccessTokenRequest : IRequest<AuthRes>
{
    public string RefreshToken { get; set; }
}

internal class RefreshAccessTokenRequestValidator : AbstractValidator<RefreshAccessTokenRequest>
{
    public RefreshAccessTokenRequestValidator()
    {
        RuleFor(x => x.RefreshToken).NotEmpty();
    }
}

internal class RefreshAccessTokenHandler : IRequestHandler<RefreshAccessTokenRequest, AuthRes>
{
    private readonly ICryptographyService _cryptographyService;
    private readonly IClockProvider _clockProvider;
    private readonly IWebTokenService _webTokenService;
    private readonly IRefreshTokenRepository _refreshTokenRepository;
    private readonly ApplicationSettings _applicationSettings;
    private readonly IObjectMapper _mapper;

    public RefreshAccessTokenHandler(
        ICryptographyService cryptographyService,
        IClockProvider clockProvider,
        IWebTokenService webTokenService,
        IRefreshTokenRepository refreshTokenRepository,
        IOptions<ApplicationSettings> applicationSettings,
        IObjectMapper mapper)
    {
        _cryptographyService = cryptographyService;
        _clockProvider = clockProvider;
        _webTokenService = webTokenService;
        _refreshTokenRepository = refreshTokenRepository;
        _applicationSettings = applicationSettings.Value;
        _mapper = mapper;
    }

    public async Task<AuthRes> Handle(RefreshAccessTokenRequest request, CancellationToken cancellationToken)
    {
        var refreshTokenEntity = await _refreshTokenRepository.GetByTokenAsync(request.RefreshToken);

        if (refreshTokenEntity == null) throw new RefreshTokenNotFoundException();
        if (refreshTokenEntity.IsExpired(_clockProvider.Now)) throw new InvalidRefreshTokenException();
        if (refreshTokenEntity.IsRevoked) throw new RefreshTokenAlreadyRevokedException();

        return await AuthCommonLogic.IssueAccessAsync(
            _refreshTokenRepository,
            _webTokenService,
            _cryptographyService,
            _clockProvider,
            _mapper,
            refreshTokenEntity.User,
            _applicationSettings.AccessTokenLifetime,
            _applicationSettings.RefreshTokenLifetime);
    }
}
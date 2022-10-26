using FluentValidation;
using Mapster;
using MediatR;
using Onion.App.Data.Database.Repositories;
using Onion.App.Logic.Auth.Exceptions;
using Onion.App.Logic.Auth.Models;
using Onion.App.Logic.Common.Attributes;
using Onion.App.Logic.Common.Security;
using Onion.Shared.Clock;

namespace Onion.App.Logic.Auth.UseCases;

[Authorize]
public class RevokeRefreshTokenRequest : IRequest<RefreshTokenRes>
{
    public string RefreshToken { get; set; }
}

internal class RevokeRefreshTokenRequestValidator : AbstractValidator<RevokeRefreshTokenRequest>
{
    public RevokeRefreshTokenRequestValidator()
    {
        RuleFor(x => x.RefreshToken).NotEmpty();
    }
}

internal class RevokeRefreshTokenHandler : IRequestHandler<RevokeRefreshTokenRequest, RefreshTokenRes>
{
    private readonly IClockProvider _clockProvider;
    private readonly IRefreshTokenRepository _refreshTokenRepository;
    private readonly ISecurityContextProvider _securityContextProvider;

    public RevokeRefreshTokenHandler(
        IClockProvider clockProvider,
        IRefreshTokenRepository refreshTokenRepository,
        ISecurityContextProvider securityContextProvider)
    {
        _clockProvider = clockProvider;
        _refreshTokenRepository = refreshTokenRepository;
        _securityContextProvider = securityContextProvider;
    }

    public async Task<RefreshTokenRes> Handle(RevokeRefreshTokenRequest request, CancellationToken cancellationToken)
    {
        var subjectId = _securityContextProvider.GetSubjectId();

        var refreshToken = await _refreshTokenRepository.GetByTokenAndUserIdAsync(request.RefreshToken, subjectId);

        if (refreshToken == null) throw new RefreshTokenNotFoundException();
        if (refreshToken.IsExpired(_clockProvider.Now)) throw new InvalidRefreshTokenException();
        if (refreshToken.IsRevoked) throw new RefreshTokenAlreadyRevokedException();

        refreshToken.IsRevoked = true;
        refreshToken = await _refreshTokenRepository.UpdateAsync(refreshToken);

        return refreshToken.Adapt<RefreshTokenRes>();
    }
}
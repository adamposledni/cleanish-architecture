using FluentValidation;
using MediatR;
using Onion.App.Data.Database.Repositories;
using Onion.App.Logic.Auth.Exceptions;
using Onion.App.Logic.Auth.Models;
using Onion.App.Logic.Common.Attributes;
using Onion.App.Logic.Common.Security;
using Onion.Shared.Clock;
using Onion.Shared.Mapper;
using System.Threading;

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
    private readonly IObjectMapper _mapper;

    public RevokeRefreshTokenHandler(
        IClockProvider clockProvider,
        IRefreshTokenRepository refreshTokenRepository,
        ISecurityContextProvider securityContextProvider,
        IObjectMapper mapper)
    {
        _clockProvider = clockProvider;
        _refreshTokenRepository = refreshTokenRepository;
        _securityContextProvider = securityContextProvider;
        _mapper = mapper;
    }

    public async Task<RefreshTokenRes> Handle(RevokeRefreshTokenRequest request, CancellationToken cancellationToken)
    {
        var subjectId = _securityContextProvider.GetSubjectId();

        var refreshTokenEntity = await _refreshTokenRepository.GetByTokenAndUserIdAsync(request.RefreshToken, subjectId);

        if (refreshTokenEntity == null) throw new RefreshTokenNotFoundException();
        if (refreshTokenEntity.IsExpired(_clockProvider.Now)) throw new InvalidRefreshTokenException();
        if (refreshTokenEntity.IsRevoked) throw new RefreshTokenAlreadyRevokedException();

        refreshTokenEntity.IsRevoked = true;
        refreshTokenEntity = await _refreshTokenRepository.UpdateAsync(refreshTokenEntity);

        return _mapper.Map<RefreshTokenRes>(refreshTokenEntity);
    }
}
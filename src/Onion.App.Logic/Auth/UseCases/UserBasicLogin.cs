using FluentValidation;
using MediatR;
using Microsoft.Extensions.Options;
using Onion.App.Data.Database.Repositories;
using Onion.App.Data.Security;
using Onion.App.Logic.Auth.Exceptions;
using Onion.App.Logic.Auth.Models;
using Onion.App.Logic.Common;
using Onion.Shared.Clock;
using System.Threading;

namespace Onion.App.Logic.Auth.UseCases;

public class UserBasicLoginRequest : IRequest<AuthRes>
{
    public string Email { get; set; }
    public string Password { get; set; }
}

internal class UserBasicLoginRequestValidator : AbstractValidator<UserBasicLoginRequest>
{
    public UserBasicLoginRequestValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty()
            .EmailAddress();
        RuleFor(x => x.Password)
            .MinimumLength(5)
            .NotEmpty();
    }
}

internal class UserBasicLoginHandler : IRequestHandler<UserBasicLoginRequest, AuthRes>
{
    private readonly IUserRepository _userRepository;
    private readonly ICryptographyService _cryptographyService;
    private readonly IClockProvider _clockProvider;
    private readonly IWebTokenService _webTokenService;
    private readonly IRefreshTokenRepository _refreshTokenRepository;
    private readonly ApplicationSettings _applicationSettings;

    public UserBasicLoginHandler(
        IUserRepository userRepository,
        ICryptographyService cryptographyService,
        IClockProvider clockProvider,
        IWebTokenService webTokenService,
        IRefreshTokenRepository refreshTokenRepository,
        IOptions<ApplicationSettings> applicationSettings)
    {
        _userRepository = userRepository;
        _cryptographyService = cryptographyService;
        _clockProvider = clockProvider;
        _webTokenService = webTokenService;
        _refreshTokenRepository = refreshTokenRepository;
        _applicationSettings = applicationSettings.Value;
    }

    public async Task<AuthRes> Handle(UserBasicLoginRequest request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByEmailAsync(request.Email);
        if (user == null || !_cryptographyService.VerifyStringHash(request.Password, user.PasswordHash, user.PasswordSalt))
        {
            throw new InvalidEmailPasswordException();
        }

        return await AuthCommonLogic.IssueAccessAsync(
            _refreshTokenRepository,
            _webTokenService,
            _cryptographyService,
            _clockProvider,
            user,
            _applicationSettings.AccessTokenLifetime,
            _applicationSettings.RefreshTokenLifetime);
    }
}
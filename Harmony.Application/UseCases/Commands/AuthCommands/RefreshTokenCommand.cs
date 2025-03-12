using Harmony.Application.Common.Interfaces;
using Harmony.Application.Models.AuthResponseModels;
using Harmony.Application.Models.DTOs;
using Harmony.Domain.Abstractions.RepositoryInterfaces;
using Harmony.Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Harmony.Application.UseCases.Commands.AuthCommands;

public class RefreshTokenCommand : IRequest<RefreshTokenResponseModel>
{
    public RefreshToken RefreshToken { get; set; }

    public RefreshTokenCommand(RefreshToken refreshToken)
    {
        RefreshToken = refreshToken;
    }
}

public class RefreshTokenCommandHandler : IRequestHandler<RefreshTokenCommand, RefreshTokenResponseModel>
{
    private readonly ITokenGenerator _tokenGenerator;
    private readonly IIdentityService _identityService;
    private readonly IRefreshTokenRepository _refreshTokenRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<RefreshTokenCommandHandler> _logger;

    public RefreshTokenCommandHandler(ITokenGenerator tokenGenerator, IIdentityService identityService, IRefreshTokenRepository refreshTokenRepository, IUnitOfWork unitOfWork, ILogger<RefreshTokenCommandHandler> logger)
    {
        _tokenGenerator = tokenGenerator;
        _identityService = identityService;
        _refreshTokenRepository = refreshTokenRepository;
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<RefreshTokenResponseModel> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
    {
        RefreshToken? storedToken;

        try
        {
            storedToken = await _refreshTokenRepository.GetRefreshToken(request.RefreshToken.Token);
        }
        catch (Exception ex)
        {
            _logger.LogError("An error occured while getting refresh token with token {Token}. Exception: {ex}", request.RefreshToken.Token, ex);

            throw;
        }

        if(storedToken == null || storedToken.ExpiresAt < DateTime.UtcNow || storedToken.IsRevoked)
        {
            _logger.LogWarning("Refresh token is invalid or expired");

            return new RefreshTokenResponseModel
            {
                IsSucceded = false
            };
        }

        storedToken.Revoke();

        try
        {
            _refreshTokenRepository.UpdateRefreshToken(storedToken);
            await _unitOfWork.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError("An error occured while revoking refresh token with token {Token}. Exception: {ex}", request.RefreshToken.Token, ex);

            throw;
        }

        UserForTokenDTO user;

        try
        {
            user = await _identityService.GetUserById(storedToken.UserId);
        }
        catch(Exception ex)
        {
            _logger.LogError("An error occured while getting user with Id {UserId}. Exception: {ex}", storedToken.UserId, ex);

            throw;
        }

        RefreshToken token = _tokenGenerator.GenerateRefreshToken(user);

        return new RefreshTokenResponseModel
        {
            IsSucceded = true,
            RefreshToken = token
        };
    }
}

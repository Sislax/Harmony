﻿using Harmony.Application.Common.Interfaces;
using Harmony.Application.Common.Interfaces.RepositoryInterfaces;
using Harmony.Application.Models.DTOs;
using Harmony.Application.Models.DTOs.AuthDTOs;
using Harmony.Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Harmony.Application.UseCases.Authentication.AuthCommands;

public record RefreshTokenCommand(string RefreshToken) : IRequest<RefreshTokenResponseDTO>;

public class RefreshTokenCommandHandler : IRequestHandler<RefreshTokenCommand, RefreshTokenResponseDTO>
{
    private readonly ITokenGenerator _tokenGenerator;
    private readonly IIdentityService _identityService;
    private readonly IRefreshTokenRepository _refreshTokenRepository;
    private readonly IQueryMaterializerFactory _queryMaterializerFactory;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<RefreshTokenCommandHandler> _logger;

    public RefreshTokenCommandHandler(ITokenGenerator tokenGenerator, IIdentityService identityService, IRefreshTokenRepository refreshTokenRepository, IQueryMaterializerFactory queryMaterializerFactory, IUnitOfWork unitOfWork, ILogger<RefreshTokenCommandHandler> logger)
    {
        _tokenGenerator = tokenGenerator;
        _identityService = identityService;
        _refreshTokenRepository = refreshTokenRepository;
        _queryMaterializerFactory = queryMaterializerFactory;
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<RefreshTokenResponseDTO> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
    {
        RefreshToken? storedToken = await _refreshTokenRepository.GetAsync(
            filter: rt => rt.Token == request.RefreshToken,
            materializer: _queryMaterializerFactory.SingleAsync<RefreshToken>(),
            cancellationToken: cancellationToken
            );

        if (storedToken == null || storedToken.ExpiresAt < DateTime.UtcNow)
        {
            _logger.LogWarning("Refresh token is invalid or expired");

            return new RefreshTokenResponseDTO
            {
                IsSucceded = false
            };
        }

        _tokenGenerator.ExtendRefreshTokenExpiration(storedToken);

        _refreshTokenRepository.Update(storedToken);

        await _unitOfWork.SaveChangesAsync();

        UserForTokenDTO user = await _identityService.GetUserByIdAsync(storedToken.UserId);

        return new RefreshTokenResponseDTO
        {
            IsSucceded = true,
            AccessToken = _tokenGenerator.GenerateJwtToken(user),
            RefreshToken = storedToken.Token
        };
    }
}

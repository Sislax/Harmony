using System.Security.Claims;
using Harmony.Application.Common.Interfaces;
using Harmony.Domain.Abstractions.RepositoryInterfaces;
using Harmony.Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Harmony.Application.UseCases.Authentication.AuthCommands;

public record LogoutCommand(Claim UserId) : IRequest<bool>;

public class LogoutCommandHandler : IRequestHandler<LogoutCommand, bool>
{
    private readonly IRefreshTokenRepository _refreshTokenRepository;
    private readonly IUnitOfWork _unitoOfWork;
    private readonly ILogger<LogoutCommandHandler> _logger;

    public LogoutCommandHandler(IRefreshTokenRepository refreshTokenRepository, IUnitOfWork unitoOfWork, ILogger<LogoutCommandHandler> logger)
    {
        _refreshTokenRepository = refreshTokenRepository;
        _unitoOfWork = unitoOfWork;
        _logger = logger;
    }

    public async Task<bool> Handle(LogoutCommand request, CancellationToken cancellationToken)
    {
        List<RefreshToken>? refreshTokens = await _refreshTokenRepository.GetAllUserRefreshToken(request.UserId.Value);

        if (refreshTokens == null)
        {
            _logger.LogWarning("No refresh tokens found for user with id: {UserId}", request.UserId.Value);

            return false;
        }

        _refreshTokenRepository.RemoveRange(refreshTokens);
        await _unitoOfWork.SaveChangesAsync();

        _logger.LogInformation("Refresh tokens deleted for user with id: {UserId}", request.UserId.Value);

        return true;
    }
}

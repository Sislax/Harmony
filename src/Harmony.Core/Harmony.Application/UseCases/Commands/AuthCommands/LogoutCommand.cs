using System.Security.Claims;
using Harmony.Application.Common.Interfaces;
using Harmony.Domain.Abstractions.RepositoryInterfaces;
using Harmony.Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Harmony.Application.UseCases.Commands.AuthCommands;

public class LogoutCommand : IRequest<bool>
{
    public Claim UserId { get; set; }

    public LogoutCommand(Claim userId)
    {
        UserId = userId;
    }
}

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
        List<RefreshToken>? refreshTokens;

        try
        {
            refreshTokens = await _refreshTokenRepository.GetAllUserRefreshToken(request.UserId.Value);
        }
        catch (Exception ex)
        {
            _logger.LogError("Error deleting refresh tokens for user with id: {UserId}. Exception: {ex}", request.UserId.Value, ex);

            throw;
        }

        if(refreshTokens == null)
        {
            _logger.LogWarning("No refresh tokens found for user with id: {UserId}", request.UserId.Value);

            return false;
        }

        try
        {
            _refreshTokenRepository.RemoveRange(refreshTokens);
            await _unitoOfWork.SaveChangesAsync();
        }
        catch(Exception ex)
        {
            _logger.LogError("An error occured while deleting tokens for user with id: {UserId}. Exception: {ex}", request.UserId.Value, ex);

            throw;
        }

        _logger.LogInformation("Refresh tokens deleted for user with id: {UserId}", request.UserId.Value);

        return true;
    }
}

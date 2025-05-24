using System.Security.Claims;
using Harmony.Application.Common.Interfaces;
using Harmony.Application.Common.Interfaces.RepositoryInterfaces;
using Harmony.Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Harmony.Application.UseCases.Authentication.AuthCommands;

public record LogoutCommand(Claim UserId) : IRequest<bool>;

public class LogoutCommandHandler : IRequestHandler<LogoutCommand, bool>
{
    private readonly IRefreshTokenRepository _refreshTokenRepository;
    private readonly IQueryMaterializerFactory _queryMaterializerFactory;
    private readonly IUnitOfWork _unitoOfWork;
    private readonly ILogger<LogoutCommandHandler> _logger;

    public LogoutCommandHandler(IRefreshTokenRepository refreshTokenRepository, IQueryMaterializerFactory queryMaterializerFactory, IUnitOfWork unitoOfWork, ILogger<LogoutCommandHandler> logger)
    {
        _refreshTokenRepository = refreshTokenRepository;
        _queryMaterializerFactory = queryMaterializerFactory;
        _unitoOfWork = unitoOfWork;
        _logger = logger;
    }

    public async Task<bool> Handle(LogoutCommand request, CancellationToken cancellationToken)
    {
        List<RefreshToken> refreshTokens = await _refreshTokenRepository.GetAsync(
            filter: rt => rt.UserId == request.UserId.Value,
            materializer: _queryMaterializerFactory.ToListAsync<RefreshToken>(),
            cancellationToken: cancellationToken
            );

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

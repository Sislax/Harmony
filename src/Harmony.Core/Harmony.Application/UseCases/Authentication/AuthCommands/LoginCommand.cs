using Harmony.Application.Common.Exceptions.UserExceptions;
using Harmony.Application.Common.Interfaces;
using Harmony.Application.Models.DTOs;
using Harmony.Application.Models.DTOs.AuthDTOs;
using Harmony.Domain.Abstractions.RepositoryInterfaces;
using Harmony.Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Harmony.Application.UseCases.Authentication.AuthCommands;

public record LoginCommand(LoginRequestDTO LoginDTO) : IRequest<LoginResponseDTO>;

public class LoginCommandHandler : IRequestHandler<LoginCommand, LoginResponseDTO>
{
    private readonly ITokenGenerator _tokenGenerator;
    private readonly IIdentityService _identityService;
    private readonly IRefreshTokenRepository _refreshTokenRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<LoginCommandHandler> _logger;

    public LoginCommandHandler(ITokenGenerator tokenGenerator, IIdentityService identityService, IRefreshTokenRepository refreshTokenRepository,
        IUnitOfWork unitOfWork, ILogger<LoginCommandHandler> logger)
    {
        _tokenGenerator = tokenGenerator;
        _identityService = identityService;
        _refreshTokenRepository = refreshTokenRepository;
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<LoginResponseDTO> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        LoginResponseDTO result = await _identityService.SignInUserAsync(
            new LoginRequestDTO
            {
                Email = request.LoginDTO.Email,
                Password = request.LoginDTO.Password
            }
        );

        if (!result.IsSucceded)
        {
            _logger.LogWarning("User with email {Email} failed to login", request.LoginDTO.Email);

            throw new UserIdentityException("User");
        }

        UserForTokenDTO user = await _identityService.GetUserByEmailAsync(request.LoginDTO.Email);

        string token = _tokenGenerator.GenerateJwtToken(user);

        RefreshToken refreshToken = _tokenGenerator.GenerateRefreshToken(user);

        // Saving Refresh Token in the database
        _refreshTokenRepository.InsertRefreshToken(refreshToken);

        await _unitOfWork.SaveChangesAsync();

        _logger.LogInformation("User with email {Email} has logged in", request.LoginDTO.Email);

        result.Token = token;
        result.RefreshToken = refreshToken.Token;

        return result;
    }
}

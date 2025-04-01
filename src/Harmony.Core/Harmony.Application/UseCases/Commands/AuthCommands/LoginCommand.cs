using Harmony.Application.Common.Interfaces;
using Harmony.Application.Models.AuthResponseModels;
using Harmony.Application.Models.DTOs;
using Harmony.Domain.Abstractions.RepositoryInterfaces;
using Harmony.Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Harmony.Application.UseCases.Commands.AuthCommands;

public class LoginCommand : IRequest<LoginResponseModel>
{
    public LoginRequestModel LoginDTO { get; set; }

    public LoginCommand(LoginRequestModel loginDTO)
    {
        LoginDTO = loginDTO;
    }
}

public class LoginCommandHandler : IRequestHandler<LoginCommand, LoginResponseModel>
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

    public async Task<LoginResponseModel> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        LoginResponseModel result = await _identityService.SignInUserAsync(
            new LoginRequestModel
            {
                Email = request.LoginDTO.Email,
                Password = request.LoginDTO.Password
            }
        );

        if (!result.IsSucceded)
        {
            _logger.LogWarning("User with email {Email} failed to login", request.LoginDTO.Email);

            return result;
        }

        UserForTokenDTO user = await _identityService.GetUserByEmailAsync(request.LoginDTO.Email);

        string token = _tokenGenerator.GenerateJwtToken(user);
        RefreshToken refreshToken = _tokenGenerator.GenerateRefreshToken(user);

        try
        {
            // Saving Refresh Token in the database
            _refreshTokenRepository.InsertRefreshToken(refreshToken);

            await _unitOfWork.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError("An error occured while saving refresh token for user with email {Email}. Exception: {ex}", request.LoginDTO.Email, ex);

            throw;
        }

        _logger.LogInformation("User with email {Email} has logged in", request.LoginDTO.Email);

        result.UserId = user.Id;
        result.Token = token;
        result.RefreshToken = refreshToken.Token;

        return result;
    }
}

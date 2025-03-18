using Harmony.Application.Common.Interfaces;
using Harmony.Application.Models.AuthResponseModels;
using Harmony.Domain.Abstractions.RepositoryInterfaces;
using Harmony.Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Harmony.Application.UseCases.Commands.AuthCommands;

public class RegisterCommand : IRequest<RegisterResponseModel>
{
    public RegisterRequestModel RegisterDTO { get; set; }

    public RegisterCommand(RegisterRequestModel registerDTO)
    {
        RegisterDTO = registerDTO;
    }
}

public class RegisterCommandHandler : IRequestHandler<RegisterCommand, RegisterResponseModel>
{
    private readonly IIdentityService _identityService;
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<RegisterCommandHandler> _logger;

    public RegisterCommandHandler(IIdentityService identityService, IUserRepository userRepository, IUnitOfWork unitOfWork, ILogger<RegisterCommandHandler> logger)
    {
        _identityService = identityService;
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
        _logger = logger;
    }
    public async Task<RegisterResponseModel> Handle(RegisterCommand request, CancellationToken cancellationToken)
    {
        RegisterResponseModel result;

        try
        {
            result = await _identityService.CreateUserAsync(
                new RegisterRequestModel
                {
                    Id = Guid.NewGuid().ToString(),
                    FirstName = request.RegisterDTO.FirstName,
                    LastName = request.RegisterDTO.LastName,
                    Username = request.RegisterDTO.Username,
                    Email = request.RegisterDTO.Email,
                    Password = request.RegisterDTO.Password
                }
            );
        }
        catch (Exception ex)
        {
            _logger.LogError("An error occured while registering user with email {Email}. Exception: {ex}", request.RegisterDTO.Email, ex);

            throw;
        }
         

        if(!result.IsSucceded)
        {
            _logger.LogWarning("User with email {Email} failed to register", request.RegisterDTO.Email);

            return result;
        }

        try
        {
            _userRepository.InsertUser(
                new User
                {
                    Id = result.UserId!, // Suppressing Warning because we are sure that userId is not null
                    FirstName = request.RegisterDTO.FirstName,
                    LastName = request.RegisterDTO.LastName,
                    Username = request.RegisterDTO.Username,
                    Email = request.RegisterDTO.Email,
                    CreatedAt = DateTime.Now
                }
            );
            await _unitOfWork.SaveChangesAsync();
        }
        catch(Exception ex)
        {
            _logger.LogError("An error occured while inserting user with email {Email} to the database. Exception: {ex}", request.RegisterDTO.Email, ex);

            throw;
        }

        _logger.LogInformation("User with email {Email} has registered", request.RegisterDTO.Email);

        return result;
    }
}

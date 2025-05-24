using Harmony.Application.Common.Exceptions.UserExceptions;
using Harmony.Application.Common.Interfaces;
using Harmony.Application.Common.Interfaces.RepositoryInterfaces;
using Harmony.Application.Models.DTOs.AuthDTOs;
using Harmony.Domain.Abstractions.RepositoryInterfaces;
using Harmony.Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Harmony.Application.UseCases.Authentication.AuthCommands;

public record RegisterCommand(RegisterRequestDTO RegisterDTO) : IRequest<RegisterResponseDTO>;

public class RegisterCommandHandler : IRequestHandler<RegisterCommand, RegisterResponseDTO>
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
    public async Task<RegisterResponseDTO> Handle(RegisterCommand request, CancellationToken cancellationToken)
    {
        if(await _userRepository.GetUserByEmail(request.RegisterDTO.Email) != null)
        {
            throw new UserDuplicateException($"User with email {request.RegisterDTO.Email} already exists.");
        }

        await _unitOfWork.BeginTransactionAsync();

        // Creates the ApplicationUser (IdentityUser)
        RegisterResponseDTO createUserResult = await _identityService.CreateUserAsync(
                new RegisterRequestDTO
                {
                    Id = request.RegisterDTO.Id,
                    FirstName = request.RegisterDTO.FirstName,
                    LastName = request.RegisterDTO.LastName,
                    Username = request.RegisterDTO.Username,
                    Email = request.RegisterDTO.Email,
                    Password = request.RegisterDTO.Password
                }
            );

        _logger.LogInformation("ApplicatonUser created correctly.");

        // If creation is not succeded, rollback and return failure response
        if (!createUserResult.IsSucceded)
        {
            await _unitOfWork.RollbackTransactionAsync();

            _logger.LogError("Error during user creation. Transaction rollback executed.");

            throw new UserIdentityException("An unknown error has occurred during the user creation");
        }

        // Creates the DomainUser
        _userRepository.InsertUser(
            new User
            {
                Id = request.RegisterDTO.Id,
                FirstName = request.RegisterDTO.FirstName,
                LastName = request.RegisterDTO.LastName,
                Username = request.RegisterDTO.Username,
                Email = request.RegisterDTO.Email
            }
        );

        try
        {
            // Assign Default Role to User
            await _identityService.AssignRoleAsync("RegularUser", request.RegisterDTO.Email);
        }
        catch (Exception ex)
        {
            _logger.LogError("Error during the assignment of the default role. Exception: {ex}", ex);

            await _unitOfWork.RollbackTransactionAsync();

            throw new UserIdentityException($"{ex.Message}");
        }

        await _unitOfWork.SaveChangesAsync();

        await _unitOfWork.CommitTransactionAsync();

        _logger.LogInformation("User with email {Email} has registered", request.RegisterDTO.Email);

        return createUserResult;
    }
}

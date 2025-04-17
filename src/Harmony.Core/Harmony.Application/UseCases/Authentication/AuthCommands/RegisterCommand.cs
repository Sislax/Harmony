using System.Data.Common;
using System.Transactions;
using Harmony.Application.Common.Exceptions;
using Harmony.Application.Common.Interfaces;
using Harmony.Application.Models.AuthResponseModels;
using Harmony.Domain.Abstractions.RepositoryInterfaces;
using Harmony.Domain.Entities;
using Harmony.Domain.Enums;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Harmony.Application.UseCases.Authentication.AuthCommands;

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
        if(await _userRepository.GetUserByEmail(request.RegisterDTO.Email) != null)
        {
            throw new UserDuplicateException($"User with email {request.RegisterDTO.Email} already exists.");
        }

        await _unitOfWork.BeginTransactionAsync();

        // Creates the ApplicationUser (IdentityUser)
        RegisterResponseModel createUserResult = await _identityService.CreateUserAsync(
                new RegisterRequestModel
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

        // Assign Default Role to User
        bool assignRoleResult = await _identityService.AssignRoleAsync(UserRole.User.ToString(), request.RegisterDTO.Email);

        // If assignment is not succeded, rollback and return failure response
        if (!assignRoleResult)
        {
            await _unitOfWork.RollbackTransactionAsync();

            _logger.LogError("Error during role assignment to the user. Transaction rollback executed.");

            throw new UserIdentityException("An unknown error has occurred during the user creation");
        }

        await _unitOfWork.SaveChangesAsync();

        await _unitOfWork.CommitTransactionAsync();

        _logger.LogInformation("User with email {Email} has registered", request.RegisterDTO.Email);

        return createUserResult;
    }
}

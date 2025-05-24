using Harmony.Application.Common.Interfaces;
using Harmony.Application.Common.Interfaces.RepositoryInterfaces;
using Harmony.Application.Models.DTOs.DomainDTOs.ServerDTOs;
using Harmony.Domain.Entities;
using Harmony.Domain.Enums;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Harmony.Application.UseCases.ServersManagement.ServersManagementCommands;

public record CreateServerCommand(CreateServerRequestDTO CreateServerDTO) : IRequest;

public class CreateServerCommandHandler : IRequestHandler<CreateServerCommand>
{
    private readonly IServerRepository _serverRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<CreateServerCommandHandler> _logger;

    public CreateServerCommandHandler(IServerRepository serverRepository, IUnitOfWork unitOfWork, ILogger<CreateServerCommandHandler> logger)
    {
        _serverRepository = serverRepository;
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task Handle(CreateServerCommand request, CancellationToken cancellationToken)
    {
        Guid idServer = Guid.NewGuid();

        Server newServer = new()
        {
            Id = idServer,
            ServerName = request.CreateServerDTO.ServerName,
            ServerMembers =
            [
                new ServerMember()
                {
                    ServerId = idServer,
                    // We are sure this OwnerId will be passed by the controller. The exception is just to suppress the warning
                    UserId = request.CreateServerDTO.OwnerId ?? throw new ArgumentNullException("Ops... Something went wrong. Try later..."),
                    UserRole = UserRole.Owner
                }
            ],
            Channels =
            [
                new Channel()
                {
                    ServerId = idServer,
                    Id = Guid.NewGuid(),
                    ChannelName = "Generale",
                    ChannelType = ChannelType.Text,
                }
            ]
        };

        _serverRepository.Add(newServer);

        await _unitOfWork.SaveChangesAsync();

        _logger.LogInformation("Server creation successfully completed.");
    }
}

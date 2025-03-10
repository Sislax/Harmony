using Harmony.Application.Common.Interfaces;
using Harmony.Application.Models.AuthResponseModels;
using Harmony.Application.Models.DTOs;
using MediatR;

namespace Harmony.Application.CommandsQueries.Commands.AuthCommands
{
    public class RegisterCommand : IRequest<RegisterResponseModel>
    {
        public required RegisterDTO RegisterDTO { get; set; }
    }

    public class RegisterCommandHandler : IRequestHandler<RegisterCommand, RegisterResponseModel>
    {
        private readonly IIdentityService _identityService;
        public RegisterCommandHandler(IIdentityService identityService)
        {
            _identityService = identityService;
        }
        public async Task<RegisterResponseModel> Handle(RegisterCommand request, CancellationToken cancellationToken)
        {
            bool result = await _identityService.CreateUserAsync(
                new RegisterDTO
                {
                    FirstName = request.RegisterDTO.FirstName,
                    LastName = request.RegisterDTO.LastName,
                    Username = request.RegisterDTO.Username,
                    Email = request.RegisterDTO.Email,
                    Password = request.RegisterDTO.Password
                }
            );

            return new RegisterResponseModel
            {
                IsSucceded = result
            };
        }
    }
}

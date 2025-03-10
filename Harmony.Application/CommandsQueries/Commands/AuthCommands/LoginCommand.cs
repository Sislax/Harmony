using Harmony.Application.Common.Interfaces;
using Harmony.Application.Models.AuthResponseModels;
using Harmony.Application.Models.DTOs;
using MediatR;

namespace Harmony.Application.CommandsQueries.Commands.AuthCommands
{
    public class LoginCommand : IRequest<LoginResponseModel>
    {
        public required LoginDTO LoginDTO { get; set; }
    }

    public class LoginCommandHandler : IRequestHandler<LoginCommand, LoginResponseModel>
    {
        private readonly ITokenGenerator _tokenGenerator;
        private readonly IIdentityService _identityService;

        public LoginCommandHandler(ITokenGenerator tokenGenerator, IIdentityService identityService)
        {
            _tokenGenerator = tokenGenerator;
            _identityService = identityService;
        }

        public async Task<LoginResponseModel> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            bool result = await _identityService.SignInUserAsync(
                new LoginDTO
                {
                    Email = request.LoginDTO.Email,
                    Password = request.LoginDTO.Password
                }
            );

            if (!result)
            {
                return new LoginResponseModel
                {
                    IsSucceded = false
                };
            }

            UserForTokenDTO user = await _identityService.GetUserByEmail(request.LoginDTO.Email);

            TokensDTO tokens = _tokenGenerator.GenerateTokensAsync(user);

            return new LoginResponseModel
            {
                IsSucceded = true,
                UserId = user.Id,
                Token = tokens.AccessToken,
                RefreshToken = tokens.RefreshToken
            };
        }
    }
}

using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Harmony.Application.Common.Interfaces;
using Harmony.Application.Models.DTOs;
using Harmony.Domain.Entities;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;

namespace Harmony.Infrastructure.Services;

public class TokenGenerator : ITokenGenerator
{
    private readonly string _key;
    private readonly string _issuer;
    private readonly string _audience;
    private readonly int _tokenExpirationMinutes;
    private readonly ILogger<TokenGenerator> _logger;

    public TokenGenerator(string key, string issueer, string audience, int tokenExpirationMinutes, ILogger<TokenGenerator> logger)
    {
        _key = key;
        _issuer = issueer;
        _audience = audience;
        _tokenExpirationMinutes = tokenExpirationMinutes;
        _logger = logger;
    }

    public TokensDTO GenerateTokensAsync(UserForTokenDTO user)
    {
        SymmetricSecurityKey securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_key));
        SigningCredentials signinCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        List<Claim> claims = new List<Claim>()
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Username),
            new Claim(JwtRegisteredClaimNames.Email, user.Email),
            new Claim(JwtRegisteredClaimNames.Jti, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.Username),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
        };

        JwtSecurityToken token = new JwtSecurityToken(
            issuer: _issuer,
            audience: _audience,
            claims: claims,
            expires: DateTime.Now.AddMinutes(_tokenExpirationMinutes),
            signingCredentials: signinCredentials
            );

        string encodedToken = new JwtSecurityTokenHandler().WriteToken(token);

        RefreshToken refreshToken = GenerateRefreshToken(user);

        _logger.LogInformation("Generated new tokens for user {user.Username}", user.Username);

        return new TokensDTO
        {
            AccessToken = encodedToken,
            RefreshToken = refreshToken
        };
    }

    public RefreshToken GenerateRefreshToken(UserForTokenDTO user)
    {
        return new RefreshToken
        {
            Id = Guid.NewGuid(),
            UserId = user.Id,
            Token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64)),
            ExpiresAt = DateTime.Now.AddDays(7)
        };
    }
}

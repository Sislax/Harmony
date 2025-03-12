using Harmony.Domain.Entities;

namespace Harmony.Application.Models.DTOs;

public class TokensDTO
{
    public required string AccessToken { get; set; }
    public required RefreshToken RefreshToken { get; set; }
}

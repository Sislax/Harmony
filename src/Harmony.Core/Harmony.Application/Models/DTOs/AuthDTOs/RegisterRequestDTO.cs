namespace Harmony.Application.Models.DTOs.AuthDTOs;

public class RegisterRequestDTO
{
    public required string Id { get; set; }
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public required string Username { get; set; }
    public required string Email { get; set; }
    public required string Password { get; set; }
}

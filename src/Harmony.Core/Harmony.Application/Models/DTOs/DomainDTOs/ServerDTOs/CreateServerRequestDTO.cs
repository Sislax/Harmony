namespace Harmony.Application.Models.DTOs.DomainDTOs.ServerDTOs;

public class CreateServerRequestDTO
{
    public string ServerName { get; set; }
    public string? OwnerId { get; set; }

    public CreateServerRequestDTO(string serverName, string? ownerId = null)
    {
        ServerName = serverName;
        OwnerId = ownerId;
    }
}

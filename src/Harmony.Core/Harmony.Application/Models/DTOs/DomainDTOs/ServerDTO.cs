namespace Harmony.Application.Models.DTOs.DomainDTOs;

public class ServerDTO
{
    public string ServerName { get; set; }
    public string? OwnerId { get; set; }

    public ServerDTO(string serverName, string? ownerId = null)
    {
        ServerName = serverName;
        OwnerId = ownerId;
    }
}

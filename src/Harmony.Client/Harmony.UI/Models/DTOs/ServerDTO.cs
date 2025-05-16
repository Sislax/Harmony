namespace Harmony.UI.Models.DTOs;

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

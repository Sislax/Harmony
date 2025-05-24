namespace Harmony.Application.Models.DTOs.DomainDTOs.ServerDTOs;

public class GetServersByUserResponseDTO
{
    public Guid Id { get; set; }
    public string ServerName { get; set; }

    public GetServersByUserResponseDTO(Guid id, string serverName)
    {
        Id = id;
        ServerName = serverName;
    }
}

using Harmony.Application.Common.Interfaces.RepositoryInterfaces;
using Harmony.Domain.Abstractions.RepositoryInterfaces;
using Harmony.Domain.Entities;
using Harmony.Infrastructure.Data;
using Microsoft.Extensions.Logging;

namespace Harmony.Infrastructure.Repositories;

public class ChannelRepository : IChannelRepository
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<ChannelRepository> _logger;

    public ChannelRepository(ApplicationDbContext context, ILogger<ChannelRepository> logger)
    {
        _context = context;
        _logger = logger;
    }

    public void Add(Channel newChannel)
    {
        _context.Channels.Add(newChannel);
    }
}

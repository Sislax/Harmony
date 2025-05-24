using Harmony.Application.Common.Interfaces.RepositoryInterfaces;
using Harmony.Domain.Entities;
using Harmony.Infrastructure.Data;
using Microsoft.Extensions.Logging;

namespace Harmony.Infrastructure.Repositories;

public class ChannelRepository : GenericRepository<Channel>, IChannelRepository
{
    private readonly ILogger<ChannelRepository> _logger;

    public ChannelRepository(ApplicationDbContext context, ILogger<ChannelRepository> logger) : base(context)
    {
        _logger = logger;
    }
}

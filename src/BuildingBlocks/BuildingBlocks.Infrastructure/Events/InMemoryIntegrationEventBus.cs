using BuildingBlocks.Application.Events;
using Microsoft.Extensions.Logging;

namespace BuildingBlocks.Infrastructure.Events;

public class InMemoryIntegrationEventBus : IIntegrationEventBus
{
    private readonly ILogger<InMemoryIntegrationEventBus> _logger;

    public InMemoryIntegrationEventBus(ILogger<InMemoryIntegrationEventBus> logger)
    {
        _logger = logger;
    }

    public Task PublishAsync(IIntegrationEvent integrationEvent, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Integration event published: {EventType} {@Event}", integrationEvent.GetType().Name, integrationEvent);
        return Task.CompletedTask;
    }
}

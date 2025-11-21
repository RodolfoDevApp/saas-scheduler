using BuildingBlocks.Application.IntegrationEvents;
using Microsoft.Extensions.Logging;

namespace BuildingBlocks.Infrastructure.IntegrationEvents;

public class InMemoryIntegrationEventPublisher : IIntegrationEventPublisher
{
    private readonly ILogger<InMemoryIntegrationEventPublisher> _logger;

    public InMemoryIntegrationEventPublisher(ILogger<InMemoryIntegrationEventPublisher> logger)
    {
        _logger = logger;
    }

    public Task PublishAsync(IntegrationEvent integrationEvent, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Integration event published: {EventType} {@Event}", integrationEvent.GetType().Name, integrationEvent);
        return Task.CompletedTask;
    }
}

namespace BuildingBlocks.Application.Events;

public abstract record IntegrationEvent(Guid EventId, DateTime OccurredOnUtc) : IIntegrationEvent
{
    protected IntegrationEvent() : this(Guid.NewGuid(), DateTime.UtcNow)
    {
    }
}

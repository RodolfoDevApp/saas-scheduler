namespace BuildingBlocks.Application.IntegrationEvents;

/// <summary>
/// Base contract for all outbound integration events.
/// </summary>
/// <param name="EventId">Unique identifier for the event message.</param>
/// <param name="OccurredOn">UTC timestamp when the event was created.</param>
public abstract record IntegrationEvent(Guid EventId, DateTime OccurredOn)
{
    protected IntegrationEvent() : this(Guid.NewGuid(), DateTime.UtcNow)
    {
    }
}

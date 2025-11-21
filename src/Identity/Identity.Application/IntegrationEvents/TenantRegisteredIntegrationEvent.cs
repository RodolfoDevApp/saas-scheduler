using BuildingBlocks.Application.Events;

namespace Identity.Application.IntegrationEvents;

public record TenantRegisteredIntegrationEvent(Guid TenantId, string Name, DateTime CreatedAtUtc)
    : IntegrationEvent;

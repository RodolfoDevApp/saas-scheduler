using BuildingBlocks.Application.IntegrationEvents;

namespace Identity.Application.IntegrationEvents;

public record TenantRegisteredIntegrationEvent(Guid TenantId, string Name, DateTime CreatedAt)
    : IntegrationEvent;

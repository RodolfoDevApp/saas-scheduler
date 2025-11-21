using BuildingBlocks.Application.IntegrationEvents;

namespace Identity.Application.IntegrationEvents;

public record UserDeactivatedIntegrationEvent(Guid UserId, Guid TenantId)
    : IntegrationEvent;

using BuildingBlocks.Application.Events;

namespace Identity.Application.IntegrationEvents;

public record UserDeactivatedIntegrationEvent(Guid UserId, Guid TenantId) : IntegrationEvent;

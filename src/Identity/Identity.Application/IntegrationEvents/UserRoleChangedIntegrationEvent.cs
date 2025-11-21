using BuildingBlocks.Application.Events;

namespace Identity.Application.IntegrationEvents;

public record UserRoleChangedIntegrationEvent(Guid UserId, Guid TenantId, IReadOnlyCollection<string> Roles) : IntegrationEvent;

using BuildingBlocks.Application.Events;

namespace Identity.Application.IntegrationEvents;

public record UserRegisteredIntegrationEvent(
    Guid UserId,
    Guid TenantId,
    string Name,
    string Email,
    IReadOnlyCollection<string> Roles)
    : IntegrationEvent;

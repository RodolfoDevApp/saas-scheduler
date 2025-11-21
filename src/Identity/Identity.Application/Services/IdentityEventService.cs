using BuildingBlocks.Application.IntegrationEvents;
using Identity.Application.IntegrationEvents;

namespace Identity.Application.Services;

public class IdentityEventService
{
    private readonly IIntegrationEventPublisher _publisher;

    public IdentityEventService(IIntegrationEventPublisher publisher)
    {
        _publisher = publisher;
    }

    public Task PublishTenantRegisteredAsync(Guid tenantId, string name, DateTime createdAt, CancellationToken cancellationToken = default)
    {
        var @event = new TenantRegisteredIntegrationEvent(tenantId, name, createdAt);
        return _publisher.PublishAsync(@event, cancellationToken);
    }

    public Task PublishUserRegisteredAsync(Guid userId, Guid tenantId, string name, string email, IEnumerable<string> roles, CancellationToken cancellationToken = default)
    {
        var @event = new UserRegisteredIntegrationEvent(userId, tenantId, name, email, roles.ToArray());
        return _publisher.PublishAsync(@event, cancellationToken);
    }

    public Task PublishUserRoleChangedAsync(Guid userId, Guid tenantId, IEnumerable<string> roles, CancellationToken cancellationToken = default)
    {
        var @event = new UserRoleChangedIntegrationEvent(userId, tenantId, roles.ToArray());
        return _publisher.PublishAsync(@event, cancellationToken);
    }

    public Task PublishUserDeactivatedAsync(Guid userId, Guid tenantId, CancellationToken cancellationToken = default)
    {
        var @event = new UserDeactivatedIntegrationEvent(userId, tenantId);
        return _publisher.PublishAsync(@event, cancellationToken);
    }
}

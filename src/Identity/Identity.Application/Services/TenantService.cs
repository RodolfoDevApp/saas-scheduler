using BuildingBlocks.Application.Events;
using Identity.Application.Abstractions;
using Identity.Application.IntegrationEvents;
using Identity.Domain.Entities;

namespace Identity.Application.Services;

public class TenantService
{
    private readonly ITenantRepository _tenantRepository;
    private readonly IIntegrationEventBus _eventBus;

    public TenantService(ITenantRepository tenantRepository, IIntegrationEventBus eventBus)
    {
        _tenantRepository = tenantRepository;
        _eventBus = eventBus;
    }

    public async Task<Tenant> RegisterTenantAsync(string name, string slug, CancellationToken cancellationToken = default)
    {
        var tenant = new Tenant(Guid.NewGuid(), name, slug);
        await _tenantRepository.AddAsync(tenant, cancellationToken);

        var integrationEvent = new TenantRegisteredIntegrationEvent(tenant.Id, tenant.Name, tenant.CreatedAtUtc);
        await _eventBus.PublishAsync(integrationEvent, cancellationToken);

        return tenant;
    }
}

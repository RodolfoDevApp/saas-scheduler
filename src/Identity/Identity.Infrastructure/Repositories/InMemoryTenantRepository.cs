using Identity.Application.Abstractions;
using Identity.Domain.Entities;

namespace Identity.Infrastructure.Repositories;

public class InMemoryTenantRepository : ITenantRepository
{
    private readonly Dictionary<Guid, Tenant> _tenants = new();

    public Task AddAsync(Tenant tenant, CancellationToken cancellationToken = default)
    {
        _tenants[tenant.Id] = tenant;
        return Task.CompletedTask;
    }

    public Task<Tenant?> GetAsync(Guid id, CancellationToken cancellationToken = default)
    {
        _tenants.TryGetValue(id, out var tenant);
        return Task.FromResult<Tenant?>(tenant);
    }
}

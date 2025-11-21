using Identity.Domain.Entities;
using Identity.Domain.Repositories;

namespace Identity.Infrastructure.InMemory;

public class TenantRepository : ITenantRepository
{
    public Task<Tenant?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var tenant = InMemoryIdentityStore.Tenants.FirstOrDefault(t => t.Id == id);
        return Task.FromResult(tenant);
    }

    public Task AddAsync(Tenant tenant, CancellationToken cancellationToken = default)
    {
        InMemoryIdentityStore.Tenants.Add(tenant);
        return Task.CompletedTask;
    }
}

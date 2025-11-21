using Identity.Domain.Entities;

namespace Identity.Application.Abstractions;

public interface ITenantRepository
{
    Task AddAsync(Tenant tenant, CancellationToken cancellationToken = default);
    Task<Tenant?> GetAsync(Guid id, CancellationToken cancellationToken = default);
}

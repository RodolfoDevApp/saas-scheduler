using Identity.Domain.Entities;
using Identity.Domain.Repositories;

namespace Identity.Infrastructure.InMemory;

public class RoleRepository : IRoleRepository
{
    public Task<IReadOnlyList<Role>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        IReadOnlyList<Role> roles = InMemoryIdentityStore.Roles.ToList();
        return Task.FromResult(roles);
    }

    public Task<Role?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var role = InMemoryIdentityStore.Roles.FirstOrDefault(r => r.Id == id);
        return Task.FromResult(role);
    }

    public Task AddAsync(Role role, CancellationToken cancellationToken = default)
    {
        InMemoryIdentityStore.Roles.Add(role);
        return Task.CompletedTask;
    }
}

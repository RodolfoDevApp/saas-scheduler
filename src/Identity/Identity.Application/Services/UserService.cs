using BuildingBlocks.Application.Events;
using Identity.Application.Abstractions;
using Identity.Application.IntegrationEvents;
using Identity.Domain.Entities;

namespace Identity.Application.Services;

public class UserService
{
    private readonly IUserRepository _userRepository;
    private readonly ITenantRepository _tenantRepository;
    private readonly IIntegrationEventBus _eventBus;

    public UserService(IUserRepository userRepository, ITenantRepository tenantRepository, IIntegrationEventBus eventBus)
    {
        _userRepository = userRepository;
        _tenantRepository = tenantRepository;
        _eventBus = eventBus;
    }

    public async Task<User?> GetAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _userRepository.GetAsync(id, cancellationToken);
    }

    public async Task<User> RegisterUserAsync(
        Guid tenantId,
        string name,
        string email,
        string? phone,
        IEnumerable<string>? roles,
        CancellationToken cancellationToken = default)
    {
        var tenant = await _tenantRepository.GetAsync(tenantId, cancellationToken);
        if (tenant is null)
        {
            throw new InvalidOperationException($"Tenant {tenantId} not found");
        }

        var user = new User(Guid.NewGuid(), tenantId, name, email, phone, roles);
        await _userRepository.AddAsync(user, cancellationToken);

        var integrationEvent = new UserRegisteredIntegrationEvent(
            user.Id,
            user.TenantId,
            user.Name,
            user.Email,
            user.Roles.ToArray());
        await _eventBus.PublishAsync(integrationEvent, cancellationToken);

        return user;
    }

    public async Task<User> AddRoleAsync(Guid userId, string role, CancellationToken cancellationToken = default)
    {
        var user = await _userRepository.GetAsync(userId, cancellationToken) ??
                   throw new InvalidOperationException($"User {userId} not found");

        if (user.AddRole(role))
        {
            await _userRepository.UpdateAsync(user, cancellationToken);
            await PublishRoleChangedAsync(user, cancellationToken);
        }

        return user;
    }

    public async Task<User> RemoveRoleAsync(Guid userId, string role, CancellationToken cancellationToken = default)
    {
        var user = await _userRepository.GetAsync(userId, cancellationToken) ??
                   throw new InvalidOperationException($"User {userId} not found");

        if (user.RemoveRole(role))
        {
            await _userRepository.UpdateAsync(user, cancellationToken);
            await PublishRoleChangedAsync(user, cancellationToken);
        }

        return user;
    }

    public async Task<User> DeactivateAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        var user = await _userRepository.GetAsync(userId, cancellationToken) ??
                   throw new InvalidOperationException($"User {userId} not found");

        if (user.IsActive)
        {
            user.Deactivate();
            await _userRepository.UpdateAsync(user, cancellationToken);
            await _eventBus.PublishAsync(
                new UserDeactivatedIntegrationEvent(user.Id, user.TenantId),
                cancellationToken);
        }

        return user;
    }

    private async Task PublishRoleChangedAsync(User user, CancellationToken cancellationToken)
    {
        var roleChanged = new UserRoleChangedIntegrationEvent(user.Id, user.TenantId, user.Roles.ToArray());
        await _eventBus.PublishAsync(roleChanged, cancellationToken);
    }
}

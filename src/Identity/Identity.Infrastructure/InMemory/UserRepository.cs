using Identity.Domain.Entities;
using Identity.Domain.Repositories;

namespace Identity.Infrastructure.InMemory;

public class UserRepository : IUserRepository
{
    public Task<User?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var user = InMemoryIdentityStore.Users.FirstOrDefault(u => u.Id == id);
        return Task.FromResult(user);
    }

    public Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        var user = InMemoryIdentityStore.Users.FirstOrDefault(u => u.Email.Equals(email, StringComparison.OrdinalIgnoreCase));
        return Task.FromResult(user);
    }

    public Task<IReadOnlyList<User>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        IReadOnlyList<User> users = InMemoryIdentityStore.Users.ToList();
        return Task.FromResult(users);
    }

    public Task AddAsync(User user, CancellationToken cancellationToken = default)
    {
        InMemoryIdentityStore.Users.Add(user);
        return Task.CompletedTask;
    }

    public Task UpdateAsync(User user, CancellationToken cancellationToken = default)
    {
        var index = InMemoryIdentityStore.Users.FindIndex(u => u.Id == user.Id);
        if (index >= 0)
        {
            InMemoryIdentityStore.Users[index] = user;
        }

        return Task.CompletedTask;
    }

    public Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        InMemoryIdentityStore.Users.RemoveAll(u => u.Id == id);
        return Task.CompletedTask;
    }
}

namespace Identity.Domain.Entities;

public class User
{
    public Guid Id { get; private set; }
    public Guid TenantId { get; private set; }
    public string Name { get; private set; }
    public string Email { get; private set; }
    public string? Phone { get; private set; }
    public string PasswordHash { get; private set; }
    public bool IsActive { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public HashSet<Guid> RoleIds { get; } = new();

    public User(Guid id, Guid tenantId, string name, string email, string? phone, string passwordHash, bool isActive, DateTime createdAt)
    {
        Id = id;
        TenantId = tenantId;
        Name = name;
        Email = email;
        Phone = phone;
        PasswordHash = passwordHash;
        IsActive = isActive;
        CreatedAt = createdAt;
    }

    public static User Create(Guid tenantId, string name, string email, string? phone, string passwordHash)
    {
        return new User(Guid.NewGuid(), tenantId, name, email, phone, passwordHash, true, DateTime.UtcNow);
    }

    public void Update(string name, string email, string? phone)
    {
        Name = name;
        Email = email;
        Phone = phone;
    }

    public void Deactivate()
    {
        IsActive = false;
    }

    public void AssignRole(Guid roleId)
    {
        RoleIds.Add(roleId);
    }

    public void RemoveRole(Guid roleId)
    {
        RoleIds.Remove(roleId);
    }
}

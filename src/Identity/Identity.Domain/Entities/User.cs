namespace Identity.Domain.Entities;

public class User
{
    private readonly HashSet<string> _roles = new();

    public Guid Id { get; private set; }
    public Guid TenantId { get; private set; }
    public string Name { get; private set; }
    public string Email { get; private set; }
    public string? Phone { get; private set; }
    public bool IsActive { get; private set; }
    public DateTime CreatedAtUtc { get; private set; }
    public IReadOnlyCollection<string> Roles => _roles;

    public User(Guid id, Guid tenantId, string name, string email, string? phone, IEnumerable<string>? roles = null)
    {
        Id = id == Guid.Empty ? Guid.NewGuid() : id;
        TenantId = tenantId;
        Name = name;
        Email = email;
        Phone = phone;
        IsActive = true;
        CreatedAtUtc = DateTime.UtcNow;

        if (roles != null)
        {
            foreach (var role in roles)
            {
                _roles.Add(role);
            }
        }
    }

    public bool AddRole(string role) => _roles.Add(role);

    public bool RemoveRole(string role) => _roles.Remove(role);

    public void Deactivate() => IsActive = false;
}

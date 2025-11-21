namespace Identity.Domain.Entities;

public class User
{
    public Guid Id { get; set; }

    public Guid TenantId { get; set; }

    public string Name { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public string? Phone { get; set; }

    public string PasswordHash { get; set; } = string.Empty;

    public bool IsActive { get; set; }

    public DateTime CreatedAt { get; set; }

    public Tenant? Tenant { get; set; }

    public ICollection<UserRole> Roles { get; set; } = new HashSet<UserRole>();
}

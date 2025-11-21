namespace Identity.Application.Users.Dtos;

public class UserResponse
{
    public Guid Id { get; set; }
    public Guid TenantId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string? Phone { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
    public List<RoleSummary> Roles { get; set; } = new();
}

public class RoleSummary
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
}

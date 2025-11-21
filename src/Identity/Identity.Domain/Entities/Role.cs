namespace Identity.Domain.Entities;

public class Role
{
    public Guid Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public ICollection<UserRole> Users { get; set; } = new HashSet<UserRole>();
}

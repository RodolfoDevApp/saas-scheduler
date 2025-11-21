namespace Identity.Domain.Entities;

public class Tenant
{
    public Guid Id { get; private set; }
    public string Name { get; private set; }
    public string Slug { get; private set; }
    public bool Active { get; private set; }
    public DateTime CreatedAt { get; private set; }

    public Tenant(Guid id, string name, string slug, bool active, DateTime createdAt)
    {
        Id = id;
        Name = name;
        Slug = slug;
        Active = active;
        CreatedAt = createdAt;
    }

    public static Tenant Create(string name, string slug)
    {
        return new Tenant(Guid.NewGuid(), name, slug, true, DateTime.UtcNow);
    }
}

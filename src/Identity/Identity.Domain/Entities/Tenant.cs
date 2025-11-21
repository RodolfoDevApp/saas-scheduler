namespace Identity.Domain.Entities;

public class Tenant
{
    public Guid Id { get; private set; }
    public string Name { get; private set; }
    public string Slug { get; private set; }
    public bool Active { get; private set; }
    public DateTime CreatedAtUtc { get; private set; }

    public Tenant(Guid id, string name, string slug)
    {
        Id = id == Guid.Empty ? Guid.NewGuid() : id;
        Name = name;
        Slug = slug;
        Active = true;
        CreatedAtUtc = DateTime.UtcNow;
    }
}

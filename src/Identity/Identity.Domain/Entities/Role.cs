namespace Identity.Domain.Entities;

public class Role
{
    public Guid Id { get; private set; }
    public string Name { get; private set; }

    public Role(Guid id, string name)
    {
        Id = id;
        Name = name;
    }

    public static Role Create(string name)
    {
        return new Role(Guid.NewGuid(), name);
    }
}

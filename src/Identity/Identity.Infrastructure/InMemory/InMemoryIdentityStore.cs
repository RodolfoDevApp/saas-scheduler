using Identity.Domain.Entities;

namespace Identity.Infrastructure.InMemory;

public static class InMemoryIdentityStore
{
    public static List<Tenant> Tenants { get; } = new();
    public static List<User> Users { get; } = new();
    public static List<Role> Roles { get; } = new();

    public static void Seed()
    {
        if (Roles.Count == 0)
        {
            Roles.AddRange(new[]
            {
                new Role(Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"), "Admin"),
                new Role(Guid.Parse("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb"), "Operator"),
                new Role(Guid.Parse("cccccccc-cccc-cccc-cccc-cccccccccccc"), "Viewer")
            });
        }

        if (Tenants.Count == 0)
        {
            var tenant = new Tenant(Guid.Parse("dddddddd-dddd-dddd-dddd-dddddddddddd"), "Default Tenant", "default", true, DateTime.UtcNow);
            Tenants.Add(tenant);
        }

        if (Users.Count == 0)
        {
            var tenantId = Tenants.First().Id;
            var adminUser = new User(Guid.Parse("eeeeeeee-eeee-eeee-eeee-eeeeeeeeeeee"), tenantId, "Admin User", "admin@example.com", null, "password", true, DateTime.UtcNow);
            adminUser.AssignRole(Roles.First().Id);
            Users.Add(adminUser);
        }
    }
}

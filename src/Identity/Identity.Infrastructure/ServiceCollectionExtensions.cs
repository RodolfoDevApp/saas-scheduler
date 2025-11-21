using Identity.Application;
using Identity.Domain.Repositories;
using Identity.Infrastructure.InMemory;
using Microsoft.Extensions.DependencyInjection;

namespace Identity.Infrastructure;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddIdentityInfrastructure(this IServiceCollection services)
    {
        InMemoryIdentityStore.Seed();

        services.AddSingleton<ITenantRepository, TenantRepository>();
        services.AddSingleton<IUserRepository, UserRepository>();
        services.AddSingleton<IRoleRepository, RoleRepository>();

        services.AddIdentityApplication();
        return services;
    }
}

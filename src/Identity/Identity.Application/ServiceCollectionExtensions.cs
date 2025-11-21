using Identity.Application.Auth.Services;
using Identity.Application.Roles.Services;
using Identity.Application.Tenants.Services;
using Identity.Application.Users.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Identity.Application;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddIdentityApplication(this IServiceCollection services)
    {
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<ITenantService, TenantService>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IRoleService, RoleService>();
        return services;
    }
}

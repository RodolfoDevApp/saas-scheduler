using BuildingBlocks.Application.Events;
using BuildingBlocks.Infrastructure.Events;
using Identity.Application.Abstractions;
using Identity.Application.Services;
using Identity.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddLogging();
builder.Services.AddSingleton<IIntegrationEventBus, InMemoryIntegrationEventBus>();
builder.Services.AddSingleton<ITenantRepository, InMemoryTenantRepository>();
builder.Services.AddSingleton<IUserRepository, InMemoryUserRepository>();
builder.Services.AddScoped<TenantService>();
builder.Services.AddScoped<UserService>();

builder.Services.AddOpenApi();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.MapPost("/api/identity/tenants", async (TenantRequest request, TenantService tenantService) =>
{
    var tenant = await tenantService.RegisterTenantAsync(request.Name, request.Slug);
    return Results.Created($"/api/identity/tenants/{tenant.Id}", tenant);
});

app.MapPost("/api/identity/users", async (UserRequest request, UserService userService) =>
{
    var user = await userService.RegisterUserAsync(request.TenantId, request.Name, request.Email, request.Phone, request.Roles);
    return Results.Created($"/api/identity/users/{user.Id}", user);
});

app.MapPost("/api/identity/users/{id:guid}/roles", async (Guid id, RoleRequest request, UserService userService) =>
{
    var user = await userService.AddRoleAsync(id, request.Role);
    return Results.Ok(user);
});

app.MapDelete("/api/identity/users/{id:guid}/roles/{role}", async (Guid id, string role, UserService userService) =>
{
    var user = await userService.RemoveRoleAsync(id, role);
    return Results.Ok(user);
});

app.MapDelete("/api/identity/users/{id:guid}", async (Guid id, UserService userService) =>
{
    var user = await userService.DeactivateAsync(id);
    return Results.Ok(user);
});

app.Run();

internal record TenantRequest(string Name, string Slug);

internal record UserRequest(Guid TenantId, string Name, string Email, string? Phone, IEnumerable<string>? Roles);

internal record RoleRequest(string Role);

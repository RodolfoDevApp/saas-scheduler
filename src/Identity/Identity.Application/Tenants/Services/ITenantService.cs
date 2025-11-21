using BuildingBlocks.Application;
using Identity.Application.Tenants.Dtos;

namespace Identity.Application.Tenants.Services;

public interface ITenantService
{
    Task<Result<TenantResponse>> CreateAsync(CreateTenantRequest request, CancellationToken cancellationToken = default);
    Task<Result<TenantResponse>> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
}

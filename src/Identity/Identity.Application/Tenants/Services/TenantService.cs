using BuildingBlocks.Application;
using Identity.Application.Tenants.Dtos;
using Identity.Domain.Entities;
using Identity.Domain.Repositories;

namespace Identity.Application.Tenants.Services;

public class TenantService : ITenantService
{
    private readonly ITenantRepository _tenantRepository;

    public TenantService(ITenantRepository tenantRepository)
    {
        _tenantRepository = tenantRepository;
    }

    public async Task<Result<TenantResponse>> CreateAsync(CreateTenantRequest request, CancellationToken cancellationToken = default)
    {
        var tenant = Tenant.Create(request.Name, request.Slug);
        await _tenantRepository.AddAsync(tenant, cancellationToken);

        return Result.Success(MapToResponse(tenant));
    }

    public async Task<Result<TenantResponse>> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var tenant = await _tenantRepository.GetByIdAsync(id, cancellationToken);
        if (tenant is null)
        {
            return Result.Failure<TenantResponse>("tenant.notFound", "Tenant not found");
        }

        return Result.Success(MapToResponse(tenant));
    }

    private static TenantResponse MapToResponse(Tenant tenant)
    {
        return new TenantResponse
        {
            Id = tenant.Id,
            Name = tenant.Name,
            Slug = tenant.Slug,
            Active = tenant.Active,
            CreatedAt = tenant.CreatedAt
        };
    }
}

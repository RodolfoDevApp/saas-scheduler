using BuildingBlocks.Application;
using Identity.Application.Roles.Dtos;

namespace Identity.Application.Roles.Services;

public interface IRoleService
{
    Task<Result<IReadOnlyList<RoleResponse>>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<Result> AssignRoleAsync(Guid userId, AssignRoleRequest request, CancellationToken cancellationToken = default);
    Task<Result> RemoveRoleAsync(Guid userId, Guid roleId, CancellationToken cancellationToken = default);
}

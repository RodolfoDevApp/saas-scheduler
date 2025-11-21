using BuildingBlocks.Application;
using Identity.Application.Roles.Dtos;
using Identity.Domain.Repositories;

namespace Identity.Application.Roles.Services;

public class RoleService : IRoleService
{
    private readonly IRoleRepository _roleRepository;
    private readonly IUserRepository _userRepository;

    public RoleService(IRoleRepository roleRepository, IUserRepository userRepository)
    {
        _roleRepository = roleRepository;
        _userRepository = userRepository;
    }

    public async Task<Result<IReadOnlyList<RoleResponse>>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var roles = await _roleRepository.GetAllAsync(cancellationToken);
        var response = roles.Select(r => new RoleResponse { Id = r.Id, Name = r.Name }).ToList();
        return Result.Success<IReadOnlyList<RoleResponse>>(response);
    }

    public async Task<Result> AssignRoleAsync(Guid userId, AssignRoleRequest request, CancellationToken cancellationToken = default)
    {
        var user = await _userRepository.GetByIdAsync(userId, cancellationToken);
        if (user is null)
        {
            return Result.Failure("user.notFound", "User not found");
        }

        var role = await _roleRepository.GetByIdAsync(request.RoleId, cancellationToken);
        if (role is null)
        {
            return Result.Failure("role.notFound", "Role not found");
        }

        user.AssignRole(role.Id);
        await _userRepository.UpdateAsync(user, cancellationToken);
        return Result.Success();
    }

    public async Task<Result> RemoveRoleAsync(Guid userId, Guid roleId, CancellationToken cancellationToken = default)
    {
        var user = await _userRepository.GetByIdAsync(userId, cancellationToken);
        if (user is null)
        {
            return Result.Failure("user.notFound", "User not found");
        }

        user.RemoveRole(roleId);
        await _userRepository.UpdateAsync(user, cancellationToken);
        return Result.Success();
    }
}

using BuildingBlocks.Application;
using Identity.Application.Users.Dtos;
using Identity.Domain.Entities;
using Identity.Domain.Repositories;

namespace Identity.Application.Users.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly IRoleRepository _roleRepository;
    private readonly ITenantRepository _tenantRepository;

    public UserService(IUserRepository userRepository, IRoleRepository roleRepository, ITenantRepository tenantRepository)
    {
        _userRepository = userRepository;
        _roleRepository = roleRepository;
        _tenantRepository = tenantRepository;
    }

    public async Task<Result<IReadOnlyList<UserResponse>>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var users = await _userRepository.GetAllAsync(cancellationToken);
        var roles = await _roleRepository.GetAllAsync(cancellationToken);

        var response = users.Select(u => MapToResponse(u, roles)).ToList();
        return Result.Success<IReadOnlyList<UserResponse>>(response);
    }

    public async Task<Result<UserResponse>> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var user = await _userRepository.GetByIdAsync(id, cancellationToken);
        if (user is null)
        {
            return Result.Failure<UserResponse>("user.notFound", "User not found");
        }

        var roles = await _roleRepository.GetAllAsync(cancellationToken);
        return Result.Success(MapToResponse(user, roles));
    }

    public async Task<Result<UserResponse>> CreateAsync(CreateUserRequest request, CancellationToken cancellationToken = default)
    {
        var tenant = await _tenantRepository.GetByIdAsync(request.TenantId, cancellationToken);
        if (tenant is null)
        {
            return Result.Failure<UserResponse>("tenant.notFound", "Tenant not found");
        }

        var user = User.Create(request.TenantId, request.Name, request.Email, request.Phone, request.Password);
        await _userRepository.AddAsync(user, cancellationToken);

        var roles = await _roleRepository.GetAllAsync(cancellationToken);
        return Result.Success(MapToResponse(user, roles));
    }

    public async Task<Result<UserResponse>> UpdateAsync(Guid id, UpdateUserRequest request, CancellationToken cancellationToken = default)
    {
        var user = await _userRepository.GetByIdAsync(id, cancellationToken);
        if (user is null)
        {
            return Result.Failure<UserResponse>("user.notFound", "User not found");
        }

        user.Update(request.Name, request.Email, request.Phone);
        await _userRepository.UpdateAsync(user, cancellationToken);

        var roles = await _roleRepository.GetAllAsync(cancellationToken);
        return Result.Success(MapToResponse(user, roles));
    }

    public async Task<Result> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var user = await _userRepository.GetByIdAsync(id, cancellationToken);
        if (user is null)
        {
            return Result.Failure("user.notFound", "User not found");
        }

        user.Deactivate();
        await _userRepository.UpdateAsync(user, cancellationToken);
        return Result.Success();
    }

    private static UserResponse MapToResponse(User user, IReadOnlyList<Role> roles)
    {
        var assignedRoles = roles.Where(r => user.RoleIds.Contains(r.Id))
            .Select(r => new RoleSummary { Id = r.Id, Name = r.Name })
            .ToList();

        return new UserResponse
        {
            Id = user.Id,
            TenantId = user.TenantId,
            Name = user.Name,
            Email = user.Email,
            Phone = user.Phone,
            IsActive = user.IsActive,
            CreatedAt = user.CreatedAt,
            Roles = assignedRoles
        };
    }
}

using BuildingBlocks.Application;
using Identity.Application.Users.Dtos;

namespace Identity.Application.Users.Services;

public interface IUserService
{
    Task<Result<IReadOnlyList<UserResponse>>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<Result<UserResponse>> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<Result<UserResponse>> CreateAsync(CreateUserRequest request, CancellationToken cancellationToken = default);
    Task<Result<UserResponse>> UpdateAsync(Guid id, UpdateUserRequest request, CancellationToken cancellationToken = default);
    Task<Result> DeleteAsync(Guid id, CancellationToken cancellationToken = default);
}

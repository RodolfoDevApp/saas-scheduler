using BuildingBlocks.Application;
using Identity.Application.Auth.Dtos;
using Identity.Domain.Repositories;

namespace Identity.Application.Auth.Services;

public class AuthService : IAuthService
{
    private readonly IUserRepository _userRepository;

    public AuthService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<Result<LoginResponse>> LoginAsync(LoginRequest request, CancellationToken cancellationToken = default)
    {
        var user = await _userRepository.GetByEmailAsync(request.Email, cancellationToken);
        if (user is null || user.PasswordHash != request.Password || !user.IsActive)
        {
            return Result.Failure<LoginResponse>("auth.invalid", "Invalid credentials or inactive user");
        }

        var accessToken = Guid.NewGuid().ToString("N");
        var refreshToken = Guid.NewGuid().ToString("N");

        var response = new LoginResponse
        {
            UserId = user.Id,
            TenantId = user.TenantId,
            AccessToken = accessToken,
            RefreshToken = refreshToken
        };

        return Result.Success(response);
    }

    public async Task<Result<RefreshTokenResponse>> RefreshAsync(RefreshTokenRequest request, CancellationToken cancellationToken = default)
    {
        var user = await _userRepository.GetByIdAsync(request.UserId, cancellationToken);
        if (user is null || !user.IsActive)
        {
            return Result.Failure<RefreshTokenResponse>("auth.invalidUser", "User not found or inactive");
        }

        var response = new RefreshTokenResponse
        {
            UserId = user.Id,
            TenantId = user.TenantId,
            AccessToken = Guid.NewGuid().ToString("N"),
            RefreshToken = Guid.NewGuid().ToString("N")
        };

        return Result.Success(response);
    }
}

using BuildingBlocks.Application;
using Identity.Application.Auth.Dtos;

namespace Identity.Application.Auth.Services;

public interface IAuthService
{
    Task<Result<LoginResponse>> LoginAsync(LoginRequest request, CancellationToken cancellationToken = default);
    Task<Result<RefreshTokenResponse>> RefreshAsync(RefreshTokenRequest request, CancellationToken cancellationToken = default);
}

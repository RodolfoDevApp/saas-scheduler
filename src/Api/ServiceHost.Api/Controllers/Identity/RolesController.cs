using Identity.Application.Roles.Dtos;
using Identity.Application.Roles.Services;
using Microsoft.AspNetCore.Mvc;

namespace ServiceHost.Api.Controllers.Identity;

[ApiController]
[Route("api/identity")]
public class RolesController : ControllerBase
{
    private readonly IRoleService _roleService;

    public RolesController(IRoleService roleService)
    {
        _roleService = roleService;
    }

    [HttpGet("roles")]
    public async Task<IActionResult> GetRoles(CancellationToken cancellationToken)
    {
        var result = await _roleService.GetAllAsync(cancellationToken);
        if (result.IsFailure)
        {
            return BadRequest(result.Error);
        }

        return Ok(result.Value);
    }

    [HttpPost("users/{id:guid}/roles")]
    public async Task<IActionResult> AssignRole([FromRoute] Guid id, [FromBody] AssignRoleRequest request, CancellationToken cancellationToken)
    {
        var result = await _roleService.AssignRoleAsync(id, request, cancellationToken);
        if (result.IsFailure)
        {
            return NotFound(result.Error);
        }

        return NoContent();
    }

    [HttpDelete("users/{id:guid}/roles/{roleId:guid}")]
    public async Task<IActionResult> RemoveRole([FromRoute] Guid id, [FromRoute] Guid roleId, CancellationToken cancellationToken)
    {
        var result = await _roleService.RemoveRoleAsync(id, roleId, cancellationToken);
        if (result.IsFailure)
        {
            return NotFound(result.Error);
        }

        return NoContent();
    }
}

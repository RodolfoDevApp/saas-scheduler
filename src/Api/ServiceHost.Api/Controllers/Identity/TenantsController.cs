using Identity.Application.Tenants.Dtos;
using Identity.Application.Tenants.Services;
using Microsoft.AspNetCore.Mvc;

namespace ServiceHost.Api.Controllers.Identity;

[ApiController]
[Route("api/identity/tenants")]
public class TenantsController : ControllerBase
{
    private readonly ITenantService _tenantService;

    public TenantsController(ITenantService tenantService)
    {
        _tenantService = tenantService;
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateTenantRequest request, CancellationToken cancellationToken)
    {
        var result = await _tenantService.CreateAsync(request, cancellationToken);
        if (result.IsFailure)
        {
            return BadRequest(result.Error);
        }

        return CreatedAtAction(nameof(GetById), new { id = result.Value.Id }, result.Value);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var result = await _tenantService.GetByIdAsync(id, cancellationToken);
        if (result.IsFailure)
        {
            return NotFound(result.Error);
        }

        return Ok(result.Value);
    }
}

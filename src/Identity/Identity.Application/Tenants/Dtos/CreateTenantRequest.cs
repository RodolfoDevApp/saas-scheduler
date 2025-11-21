using System.ComponentModel.DataAnnotations;

namespace Identity.Application.Tenants.Dtos;

public class CreateTenantRequest
{
    [Required]
    [MinLength(3)]
    public string Name { get; set; } = string.Empty;

    [Required]
    [MinLength(3)]
    public string Slug { get; set; } = string.Empty;
}

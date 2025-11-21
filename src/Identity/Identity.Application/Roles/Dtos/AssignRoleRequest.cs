using System.ComponentModel.DataAnnotations;

namespace Identity.Application.Roles.Dtos;

public class AssignRoleRequest
{
    [Required]
    public Guid RoleId { get; set; }
}

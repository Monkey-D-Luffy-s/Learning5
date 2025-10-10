using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Learning5.Models.Account
{
    public class Roles : IdentityRole
    {
        [Required(ErrorMessage = "Role Name Required")]
        public string? RoleName { get; set; }
        public string? RoleId { get; set; }
    }
}

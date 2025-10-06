using System.ComponentModel.DataAnnotations;

namespace Learning5.Models.Account
{
    public class AssignRoleModel
    {
        [Required(ErrorMessage= "RoleName required")]
        public string RoleName { get; set; }
        [Required(ErrorMessage = "RoleName required")]
        public string UserName { get; set; }
    }
}

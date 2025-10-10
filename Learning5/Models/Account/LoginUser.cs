using System.ComponentModel.DataAnnotations;

namespace Learning5.Models.Account
{
    public class LoginUser
    {
        [Required(ErrorMessage = "User Name Required")]
        public string? UserName { get; set; }
        [Required(ErrorMessage = "Password Required")]
        public string? Password { get; set; }
    }
}

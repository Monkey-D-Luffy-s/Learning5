using System.ComponentModel.DataAnnotations;

namespace Learning5.Models.Account
{
    public class Designations
    {
        [Key]
        public string ? DesignationId { get; set; }
        [Required(ErrorMessage = "Designation Name required")]
        public string ? DesignationName { get; set; }
    }
}

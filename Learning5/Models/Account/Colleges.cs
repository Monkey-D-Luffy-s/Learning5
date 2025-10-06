using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Learning5.Models.Account;

namespace Learning5.Models.Account
{
    public class Colleges
    {
        [Key]
        [MaxLength(256)]
        public string? CollegeCode { get; set; }
        [Required(ErrorMessage = "College Name Required")]
        [MinLength(10, ErrorMessage = "College Name must be atleast 10 charectors")]
        public string? CollegeName { get; set; }
        [Required(ErrorMessage = "College Required")]
        public string? CollegeType { get; set; }
        [Required(ErrorMessage = "College Required")]
        public string? CollegeDescription { get; set; }
        [Required(ErrorMessage = "College Required")]
        public string? CollegeAddress { get; set; }
        //public string? UserName { get; set; }
        //[ForeignKey("UserName")]
        //public ICollection<User>? Users {  get; set; }
        public string? AddedBy { get; set; }
        public DateTime? AddedOn { get; set; } = DateTime.Now;
    }
}

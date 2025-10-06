using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Learning5.Models.Account
{
    [Index(nameof(UserName), IsUnique = true)]
    public class User : IdentityUser
    {
        [Required(ErrorMessage = "Employee Name required")]
        public string? EmployeeName { get; set; }
        [Required(ErrorMessage = "DepartmentId required")]
        public string? DepartmentId { get; set; }
        [Required(ErrorMessage = "AadhaarId required")]
        [StringLength(12, ErrorMessage = "Aadhaar number must be 12 digits")]
        public string? AadhaarId { get; set; }

        [Required(ErrorMessage = "PanId required")]
        [StringLength(10, ErrorMessage = "PAN number must be 10 digits")]
        public string? PanId { get; set; }
        [Required(ErrorMessage = "Address required")]
        public string? Address { get; set; }
        [Required(ErrorMessage = "DateOfBirth required")]
        public DateTime? DateOfBirth { get; set; }
        [Required(ErrorMessage = "BloodGroup required")]
        public string? BloodGroup { get; set; }
        public string? Passport_DocumentPath { get; set; }
        [Required(ErrorMessage = "Passport Photo is Required")]
        [NotMapped]
        public IFormFile? Passport_Document { get; set; }
        public string? Singnature_DocumentPath { get; set; }
        [Required(ErrorMessage = "Signature is Required")]
        [NotMapped]
        public IFormFile? Singnature_Document { get; set; }
        [Required(ErrorMessage = "DateOfJoining required")]
        public DateTime? DateOfJoining { get; set; }
        public bool? IsActive { get; set; } = true;
        public DateTime? CreatedDate { get; set; } = DateTime.Now;

        //public string? LeaveId { get; set; }
        //[ForeignKey("LeaveId")]
        //public ICollection<LeavesModule>? LeavesModule { get; set; }

        public string? DesignationId { get; set; }
        [ForeignKey("DesignationId")]
        public Designations? Designations { get; set; }
        public string? CollegeCode { get; set; }
        [ForeignKey("CollegeCode")]
        public Colleges? Colleges { get; set; }
        public int? TotalCasualLeaves { get; set; } = 10;
        public int? AvailedCasualLeaves { get; set; } = 0;

        public int? TotalSickLeaves { get; set; } = 10;
        public int? AvailedSickLeaves { get; set; } = 0;
        public int? TotalOptionalHolidays { get; set; } = 10;
        public int? AvailedoptionalHolidays { get; set; } = 0;
        public int? TotalEarnedLeaves { get; set; } = 10;
        public int? AvailedEarnedLeaves { get; set; } = 0;
      
    }
}

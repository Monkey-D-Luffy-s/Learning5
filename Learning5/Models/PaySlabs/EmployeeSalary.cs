using System.ComponentModel.DataAnnotations;

namespace Learning5.Models.PaySlabs
{
    public class EmployeeSalary
    {
        [Key]
        public int SalaryId { get; set; }
        public string? UserName { get; set; }
        [Required(ErrorMessage = "Basic Salary Required")]
        public decimal BasicSalary { get; set; }
        [Required(ErrorMessage = "HRA Required")]
        public decimal HRA { get; set; }
        [Required(ErrorMessage = "Allowances Required")]
        public decimal ConveyanceAllowance { get; set; }
        [Required(ErrorMessage = "Deductions Required")]
        public decimal Deductions { get; set; }
        public string? Flag { get; set; } = "N";

        [Required(ErrorMessage = "FromDate Required")]
        public DateTime? EffectiveFrom { get; set; }
        [Required(ErrorMessage = "ToDate Required")]
        public DateTime? EffectiveTo { get; set; }

    }
}

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Learning5.Models.Account
{
    public class LeavesModule : IValidatableObject
    {
        [Key]
        public string? LeaveId { get; set; }
        [Required(ErrorMessage = "Leave Type Rquired")]
        public string? LeaveType { get; set; }
        [Required(ErrorMessage = "Start Date Rquired")]
        public DateTime StartDate { get; set; }
        [Required(ErrorMessage = "End Date Rquired")]

        public DateTime EndDate { get; set; }
        [Required(ErrorMessage = "Reason Rquired")]
        public string? Reason { get; set; }

        public int TotalDays { get; set; }
        public string? Status { get; set; }

        public string? Remarks { get; set; }

        [MaxLength(256)]
        public string? UserName { get; set; }
        //[ForeignKey(name: UserName)]
        //public User? User { get; set; }
        public DateTime? AppliedDate { get; set; } = DateTime.Now;

        public string? ApprovedBy { get; set; }
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (EndDate < StartDate)
            {
                yield return new ValidationResult("End Date cannot be earlier than Start Date.", new[] { nameof(EndDate) });
            }
        }
    }
}

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Learning5.Models.Account
{
    public class TimeSheet
    {
        [Key]
        public int TimesheetId { get; set; }

        [Required]
        public string? UserId { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime Date { get; set; }

        [DataType(DataType.Time)]
        public DateTime? StartTime { get; set; }

        [DataType(DataType.Time)]
        public DateTime? EndTime { get; set; }

        [Column(TypeName = "decimal(5,2)")]
        public decimal TotalHours { get; set; }

        [Required]
        [StringLength(500)]
        public string TaskDescription { get; set; }

        public int? ProjectId { get; set; }

        [StringLength(50)]
        public string? LeaveType { get; set; }

        [StringLength(20)]
        public string Status { get; set; } = "Submitted";

        [StringLength(100)]
        public string? ApprovedBy { get; set; }

        [StringLength(500)]
        public string? Comments { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime? CreatedDate { get; set; } = DateTime.Now;

        [DataType(DataType.DateTime)]
        public DateTime? ModifiedDate { get; set; }
    }
}

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Learning5.Models.Account
{
    public class TimeSheet
    {
        [Key]
        public string TimesheetId { get; set; }

        [Required]
        public string? UserId { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime Date { get; set; }

        [Column(TypeName = "decimal(5,2)")]
        public decimal TotalHours { get; set; }

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

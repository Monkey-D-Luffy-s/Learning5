using System.ComponentModel.DataAnnotations;

namespace Learning5.Models.PaySlabs
{
    public class StaturaryDetailsEmployee
    {
        [Key]
        public int StaturaryDetailsEmployeeId { get; set; }
        public string? UserName { get; set; }
        public string? PANNumber { get; set; }
        public string? AadharNumber { get; set; }
        public string? PFNumber { get; set; }
        public string? ESINumber { get; set; }
        public string? UANNumber { get; set; }

        public string? Flag { get; set; } = "N";
    }
}

using System.ComponentModel.DataAnnotations;

namespace Learning5.Models.PaySlabs
{
    public class BankDetails
    {
        [Key]
        public int BankDetailId { get; set; }
        public string? BankName { get; set; }
        public string? AccountNumber { get; set; }
        public string? IFSCCode { get; set; }
        public string? Branch { get; set; }
        public string? UserName { get; set; }
        public string? Flag { get; set; } = "N";
    }
}

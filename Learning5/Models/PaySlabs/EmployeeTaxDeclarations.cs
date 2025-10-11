using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Learning5.Models.PaySlabs
{
    public class EmployeeTaxDeclarations
    {
        [Key]
        public int TaxDeclarationId { get; set; }
        public string? UserName { get; set; }
        public string? FinancialYear { get; set; }
        public string? InvestmentProof { get; set; }
        [NotMapped]
        public IFormFile? InvestmentProofFile { get; set; }
        public decimal DeclaredAmount { get; set; }

        public string? Flag { get; set; } = "N";
    }
}

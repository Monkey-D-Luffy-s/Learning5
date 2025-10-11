using Learning5.Models.PaySlabs;
using Microsoft.AspNetCore.Mvc;

namespace Learning5.services.Payments
{
    public interface IPaymentService
    {
        Task<string> AddBankDetails(BankDetails bankDetails);
        Task<JsonResult> GetBankDetails(string userid);
        Task<string> AddEmployeeSalaryDetails(EmployeeSalary empSalary);
        Task<JsonResult> GetEmployeeSalaryDetails(string userid);
        Task<string> AddEmployeeTaxDeclarationDetails(EmployeeTaxDeclarations empTax);
        Task<JsonResult> GetEmployeeTaxDeclarationDetails(string userid);
        Task<string> AddEmployeeStaturaryDetails(StaturaryDetailsEmployee empTax);
        Task<JsonResult> GetEmployeeStaturaryDetails(string userid);
    }
}

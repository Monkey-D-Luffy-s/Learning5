using Learning5.data;
using Learning5.Models.PaySlabs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Learning5.services.Payments
{
    public class PaymentService : IPaymentService
    {
        private readonly AppDbContext _context;
        public PaymentService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<string> AddBankDetails(BankDetails bankDetails)
        {
            try
            {
                await _context.BankDetails.AddAsync(bankDetails);
                await _context.SaveChangesAsync();
                return "Bank Details Added Successfully";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public async Task<JsonResult> GetBankDetails(string userid)
        {
            try
            {
                return new JsonResult(await _context.BankDetails
                    .AsNoTracking()
                    .Where(u => u.UserName == userid)
                    .ToListAsync());
            }
            catch (Exception)
            {

                throw;
            }
        }
        public async Task<string> AddEmployeeSalaryDetails(EmployeeSalary empSalary)
        {
            try
            {
                await _context.EmployeeSalaries.AddAsync(empSalary);
                await _context.SaveChangesAsync();
                return "Bank Details Added Successfully";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public async Task<JsonResult> GetEmployeeSalaryDetails(string userid)
        {
            try
            {
                return new JsonResult(await _context.EmployeeSalaries
                    .AsNoTracking()
                    .Where(u => u.UserName == userid)
                    .ToListAsync());
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<string> AddEmployeeTaxDeclarationDetails(EmployeeTaxDeclarations empTax)
        {
            try
            {

                await _context.EmployeeTaxDetails.AddAsync(empTax);
                await _context.SaveChangesAsync();
                return "Bank Details Added Successfully";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public async Task<JsonResult> GetEmployeeTaxDeclarationDetails(string userid)
        {
            try
            {
                return new JsonResult(await _context.EmployeeTaxDetails
                    .AsNoTracking()
                    .Where(u => u.UserName == userid)
                    .ToListAsync());
            }
            catch (Exception)
            {

                throw;
            }
        }
        public async Task<string> AddEmployeeStaturaryDetails(StaturaryDetailsEmployee empTax)
        {
            try
            {
                await _context.EmployeeStaturary.AddAsync(empTax);
                await _context.SaveChangesAsync();
                return "Staturary Details Added Successfully";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public async Task<JsonResult> GetEmployeeStaturaryDetails(string userid)
        {
            try
            {
                return new JsonResult(await _context.EmployeeStaturary
                    .AsNoTracking()
                    .Where(u => u.UserName == userid)
                    .ToListAsync());
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
